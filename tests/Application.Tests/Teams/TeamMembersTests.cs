namespace CTF.Application.Tests.Teams;

public class TeamMembersTests
{
    [Test]
    public void IsEmpty_WhenThereAreNoTeamMembers_ShouldReturnTrue()
    {
        // Arrange
        var members = new TeamMembers();

        // Act
        bool actual = members.IsEmpty();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsEmpty_WhenThereAreTeamMembers_ShouldReturnFalse()
    {
        // Arrange
        var members = new TeamMembers
        {
            new FakePlayer(id : 1, name : "Bob")
        };

        // Act
        bool actual = members.IsEmpty();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void Add_WhenMemberAlreadyExists_ShouldThrowArgumentException()
    {
        // Arrange
        var fakePlayer = new FakePlayer(id: 1, name: "Bob");
        var members = new TeamMembers
        {
            fakePlayer
        };
        int expectedMembers = 1;

        // Act
        Action act = () => members.Add(fakePlayer);

        // Asserts
        act.Should().Throw<ArgumentException>();
        members.Should().HaveCount(expectedMembers);
    }

    [Test]
    public void Add_WhenMemberDoesNotExist_ShouldNotThrowArgumentException()
    {
        // Arrange
        var fakePlayer = new FakePlayer(id: 2, name: "Alice");
        var members = new TeamMembers
        {
            new FakePlayer(id : 1, name : "Bob")
        };
        int expectedMembers = 2;

        // Act
        Action act = () => members.Add(fakePlayer);

        // Asserts
        act.Should().NotThrow<ArgumentException>();
        members.Should().HaveCount(expectedMembers);
    }

    [Test]
    public void Remove_WhenPlayerIsNotFound_ShouldThrowArgumentException()
    {
        // Arrange
        var fakePlayer = new FakePlayer(id: 1, name: "Bob");
        var members = new TeamMembers
        {
            new FakePlayer(id : 2, name : "Alice")
        };

        // Act
        Action act = () => members.Remove(fakePlayer);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Remove_WhenPlayerIsFound_ShouldNotThrowArgumentException()
    {
        // Arrange
        var fakePlayer = new FakePlayer(id: 1, name: "Bob");
        var members = new TeamMembers
        {
            fakePlayer
        };

        // Act
        Action act = () => members.Remove(fakePlayer);

        // Asserts
        act.Should().NotThrow<ArgumentException>();
        members.Should().BeEmpty();
    }
}
