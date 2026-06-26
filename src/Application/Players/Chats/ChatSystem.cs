namespace CTF.Application.Players.Chats;

public class ChatSystem(IDictionary<char, IChatMessage> chats) : ISystem
{
    /// <summary>
    /// This callback is called when a player sends a message in chat.
    /// </summary>
    /// <param name="player">The player who sent the message.</param>
    /// <param name="text">	The content of the message that the player sent.</param>
    /// <remarks>
    /// See <see href="https://www.open.mp/docs/scripting/callbacks/OnPlayerText"/>
    /// </remarks>
    /// <returns>
    /// By default, this callback sends a message containing the content of the message and the player's name. 
    /// <para/>
    /// Returning 0 will ignore this default behaviour.
    /// </returns>
    [Event]
    public bool OnPlayerText(Player player, string text)
    {
        if (player.IsInClassSelection())
        {
            player.SendClientMessage(Color.Red, Messages.ChatDisabled);
            return false;
        }

        char identifier = text[0];
        if (chats.TryGetValue(identifier, out IChatMessage chatMessage)) 
        {
            PlayerInfo sender = player.GetRequiredInfo();
            ReplaceFirstCharacter(text, newCharacter: ' ');
            bool sendMessageByDefault = !chatMessage.SendToAllPlayers(sender, text);
            ReplaceFirstCharacter(text, newCharacter: identifier);
            return sendMessageByDefault;
        }
        return true;
    }

    private unsafe void ReplaceFirstCharacter(string originalText, char newCharacter)
    {
        fixed (char* text = originalText)
        {
            text[0] = newCharacter;
        }
    }
}
