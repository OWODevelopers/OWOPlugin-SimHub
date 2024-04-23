using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.SqlServer.Server;
using NSubstitute;
using NUnit.Framework;
using OWOGame;
using OWOPluginSimHub.Application;
using OWOPluginSimHub.Domain;
using static OWOGame.Sensation;
using static WorldContextBuilder;

public class SensationBuilderTests
{
    [Test]
    public void Start_cinematic_does_not_feels_like_impact()
    {
        var mock = new MockHapticSystem();
        var sut = new Plugin(mock);

        sut.UpdateFeelingBasedOnWorld(DuringRace(speed: 100));
        sut.UpdateFeelingBasedOnWorld(OutsideRace());
        sut.UpdateFeelingBasedOnWorld(DuringRace());

        mock.Last.ToString().Should().NotBe(Ball.ToString());
    }

    [Test]
    public void Feel_impact_over_acceleration()
    {
        var mock = new MockHapticSystem();
        var sut = new Plugin(mock);

        sut.UpdateFeelingBasedOnWorld(DuringRace(speed:100));
        sut.UpdateFeelingBasedOnWorld(DuringRace(speed:20));

        mock.Last.ToString().Should().Be(Ball.ToString());
    }

    [Test]
    public void Stop_sensation_when_swift_gear()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock);

        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "2"));

        mock.Received(1).Stop();
    }

    [Test]
    public void Avoid_feeling_during_gear_swift()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock);

        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "3"));

        mock.Received(0).Send(Arg.Any<Sensation>(), Arg.Any<Muscle[]>());
    }

    [Test]
    public void Feel_impact_over_gear_shift()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock);

        sut.UpdateFeelingBasedOnWorld(DuringRace(speed: 200, gear: "2"));
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "2", speed: 0));

        mock.Received(1).Send(Arg.Any<Sensation>(), Arg.Any<Muscle[]>());
    }

    [Test]
    public async Task Gear_shifts_are_not_accumulated()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock);

        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "2"));
        sut.UpdateFeelingBasedOnWorld(DuringRace(gear: "3"));

        await Task.Delay(500);

        mock.Received(1).Stop();
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
}