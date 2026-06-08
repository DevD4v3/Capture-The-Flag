namespace CTF.Application.Tests.Fakes;

public class FakePlayer2 : Player
{
    public FakePlayer2() : base(Substitute.For<IOmpEntityProvider>(), default)
    {
    }

    public override T GetComponent<T>()
    {
        return null;
    }
}
