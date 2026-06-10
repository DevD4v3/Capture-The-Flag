namespace CTF.Application.Tests.Fakes;

public class FakePlayer3 : Player
{
    public FakePlayer3() : base(Substitute.For<IOmpEntityProvider>(), default)
    {
    }

    public bool IsAuthenticated { get; set; } = true;

    public override T GetComponent<T>()
    {
        var accountComponent = new AccountComponent(new PlayerInfo(), IsAuthenticated);
        return accountComponent as T;
    }
}
