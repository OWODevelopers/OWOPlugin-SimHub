using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using OWOPluginSimHub.Application;
using OWOPluginSimHub.Domain;
using static OWOGame.Sensation;
using static WorldContextBuilder;

public class SensationBuilderTests
{
    Plugin sut;
    MockHapticSystem mock;

    [SetUp]
    public void SetUp()
    {
        mock = new MockHapticSystem();
        sut = new Plugin(mock);
    }

    [Test]
    public void Start_cinematic_does_not_feels_like_impact()
    {
        sut.UpdateFeelingBasedOnWorld(DuringRace(speed: 100));
        sut.UpdateFeelingBasedOnWorld(OutsideRace());
        sut.UpdateFeelingBasedOnWorld(DuringRace());

        mock.Last.ToString().Should().NotBe(Ball.ToString());
    }

    [Test]
    public void Feel_impact_over_acceleration()
    {
        sut.UpdateFeelingBasedOnWorld(DuringRace(speed: 100));
        sut.UpdateFeelingBasedOnWorld(DuringRace(speed: 20));

        ShouldFeelImpact();
    }

    [Test]
    public void Stop_sensation_when_swift_gear()
    {
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "2"));

        DidNotReceive();
    }

    [Test]
    public void Avoid_feeling_during_gear_swift()
    {
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "3"));

        DidNotReceive();
    }

    [Test]
    public void Feel_impact_over_gear_shift()
    {
        sut.UpdateFeelingBasedOnWorld(DuringRace(speed: 200, gear: "2"));
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "2", speed: 0));

        DidReceive();
    }

    [Test]
    public async Task Gear_shifts_are_not_accumulated()
    {
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "2"));
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "3"));

        await Task.Delay(500);

        DidNotReceive();
    }

    [Test]
    public void Feel_initial_acceleration()
    {
        SpeedIntensity.From(new WorldContext { Speed = 31 })
            .Should().BeGreaterThan
            (
                SpeedIntensity.From(new WorldContext { Speed = 27 })
            );
    }

    void DidReceive() => mock.Last.Should().NotBeNull();
    void DidNotReceive() => mock.Last.Should().BeNull();
    void ShouldFeelImpact() => mock.Last.ToString().Should().Be(Ball.ToString());
}