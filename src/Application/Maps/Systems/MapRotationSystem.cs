namespace CTF.Application.Maps.Systems;

public class MapRotationSystem(
    IWorldService worldService,
    IDialogService dialogService,
    MapRotationService mapRotationService,
    MapCollection mapCollection,
    MapTextDrawRenderer mapTextDrawRenderer) : ISystem
{
    private int _connectedPlayers;

    [Event]
    public void OnPlayerSpawn(Player player)
    {
        mapTextDrawRenderer.Show(player);
    }

    [Event]
    public void OnPlayerConnect(Player player)
    {
        _connectedPlayers++;
        if (_connectedPlayers == 1)
            mapRotationService.StartRotationTimer();
    }

    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason) 
    {
        _connectedPlayers--;
        if (_connectedPlayers == 0)
            mapRotationService.StopRotationTimer();
    }

    [PlayerCommand("startrt")]
    public void StartRotationTimer(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        mapRotationService.StartRotationTimer();
    }

    [PlayerCommand("stoprt")]
    public void StopRotationTimer(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        mapRotationService.StopRotationTimer();
    }

    [PlayerCommand("settimeleft")]
    public void SetTimeLeft(Player player, int minutes)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator)) 
            return;

        var interval = new Minutes(minutes);
        TimeLeft timeLeft = mapRotationService.TimeLeft;
        Result result = timeLeft.SetInterval(interval);
        if(result.IsFailed)
        {
            player.SendClientMessage(Color.Red, result.Message);
            return;
        }

        mapTextDrawRenderer.UpdateTimeLeft(timeLeft);
    }

    [PlayerCommand("maps")]
    public async Task ShowMaps(Player player, string findBy = default)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        var listDialog = new ListDialog(string.Empty, "Select", "Close");
        IEnumerable<IMap> maps = string.IsNullOrEmpty(findBy) ?
            mapCollection.GetAll() :
            mapCollection.GetAll(findBy);

        IMap nextMap = mapRotationService.NextMap;
        foreach (IMap map in maps)
        {
            if (map.Id == nextMap.Id)
                listDialog.Add(text: $"{map.Name} {Color.Red}[Next Map]", tag: map.Id);
            else
                listDialog.Add(text: map.Name, tag: map.Id);
        }

        if (listDialog.Rows.Count == 0)
        {
            player.SendClientMessage(Color.Red, Messages.NoMatchFound);
            return;
        }

        listDialog.Caption = $"Maps: {listDialog.Rows.Count}/{mapCollection.Count}";
        ListDialogResponse listDialogResponse = await dialogService.ShowAsync(player, listDialog);
        if (listDialogResponse.Response == DialogResponse.LeftButton)
        {
            int selectedMapId = (int)listDialogResponse.Item.Tag;
            IMap selectedMap = mapCollection.GetById(selectedMapId).Value;
            await ShowConfirmationDialog(player, selectedMap);
        }
    }

    private async Task ShowConfirmationDialog(Player player, IMap selectedMap)
    {
        var confirmationDialog = new MessageDialog(
            caption: "Confirmation",
            content: "Do you want to force the map change right now?",
            button1: "Yes",
            button2: "No"
        );
        MessageDialogResponse confirmationDialogResponse = await dialogService.ShowAsync(player, confirmationDialog);
        if (confirmationDialogResponse.Response == DialogResponse.Disconnected)
            return;
        
        if (mapRotationService.IsMapLoading)
        {
            player.SendClientMessage(Color.Red, Messages.MapIsLoading);
            return;
        }

        if (confirmationDialogResponse.Response == DialogResponse.LeftButton)
        {
            TimeLeft timeLeft = mapRotationService.TimeLeft;
            timeLeft.SetInterval(new Minutes(0));
            mapTextDrawRenderer.UpdateTimeLeft(timeLeft);
            var message = Smart.Format(Messages.MapChangeForced, new
            {
                PlayerName = player.Name,
                MapName = selectedMap.Name
            });
            worldService.SendClientMessage(Color.Orange, message);
            mapRotationService.ForceNextMap(selectedMap);
        }
        else if(confirmationDialogResponse.Response == DialogResponse.RightButtonOrCancel)
        {
            var message = Smart.Format(Messages.NextMapSelection, new
            {
                PlayerName = player.Name,
                MapName = selectedMap.Name
            });
            worldService.SendClientMessage(Color.Orange, message);
            mapRotationService.ForceNextMap(selectedMap);
        }
    }
}
