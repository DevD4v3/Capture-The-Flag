namespace CTF.Application.Tests.Teams.Flags;

public class FakeCarrier : Player
{
    public FakeCarrier() : base(Substitute.For<IOmpEntityProvider>(), default)
    {
        
    }

    public override bool SetAttachedObject(
        int index,
        int modelId,
        Bone bone,
        Vector3 offset,
        Vector3 rotation,
        Vector3 scale,
        Color materialColor1,
        Color materialColor2) => true;

    public override bool RemoveAttachedObject(int index) => true;
}
