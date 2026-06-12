namespace CTF.Application.Tests.Fakes;

public class FakeMap(
    int id = 0,
    string name = "RC_Battlefield") : IMap
{
    public int Id { get; } = id;

    public string Name { get; } = name;
}
