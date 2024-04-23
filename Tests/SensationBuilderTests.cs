using System.Threading.Tasks;
using FluentAssertions;
using OWOPlugin;
using NSubstitute;
using NUnit.Framework;
using OWOGame;

public class SensationBuilderTests
{
    [Test]
    public void Start_cinematic_does_not_feels_like_impact()
    {
        var mock = new MockHapticSystem();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Speed = 100, IsRaceOn = true }
        };

        sut.UpdateFeelingBasedOnWorld();

        sut.Data = new WorldContext() { Speed = 0, IsRaceOn = false };

        sut.UpdateFeelingBasedOnWorld();

        sut.Data = new WorldContext() { Speed = 0, IsRaceOn = true };

        sut.UpdateFeelingBasedOnWorld();

        mock.Last.ToString().Should().NotBe(Sensation.Ball.ToString());
    }

    [Test]
    public void Feel_impact_over_acceleration()
    {
        var mock = Substitute.For<HapticSystem>();

        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Speed = 100, IsRaceOn = true }
        };

        sut.Data = new WorldContext() { Speed = 20, IsRaceOn = true };

        sut.UpdateFeelingBasedOnWorld();

        mock.Received(1).Send(Arg.Any<Sensation>(), Arg.Any<Muscle[]>());
    }

    [Test]
    public void Stop_sensation_when_swift_gear()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Gear = "2", IsRaceOn = true }
        };

        sut.UpdateFeelingBasedOnWorld();

        mock.Received(1).Stop();
    }

    [Test]
    public void Avoid_feeling_during_gear_swift()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Gear = "2", IsRaceOn = true }
        };

        sut.UpdateFeelingBasedOnWorld();

        sut.Data = new WorldContext() { Gear = "2", IsRaceOn = true };

        sut.UpdateFeelingBasedOnWorld();

        mock.Received(0).Send(Arg.Any<Sensation>(), Arg.Any<Muscle[]>());
    }

    [Test]
    public void Feel_impact_over_gear_shift()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Gear = "2", IsRaceOn = true, Speed = 200 }
        };

        sut.UpdateFeelingBasedOnWorld();

        sut.Data = new WorldContext() { Gear = "2", IsRaceOn = true, Speed = 0, CurrentEngineRpm = 1 };

        sut.UpdateFeelingBasedOnWorld();

        mock.Received(1).Send(Arg.Any<Sensation>(), Arg.Any<Muscle[]>());
    }

    [Test]
    public async Task Gear_shifts_are_not_accumulated()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Gear = "2", IsRaceOn = true }
        };

        sut.UpdateFeelingBasedOnWorld();

        sut.Data = new WorldContext() { Gear = "3", IsRaceOn = true };

        sut.UpdateFeelingBasedOnWorld();

        await Task.Delay(500);

        mock.Received(1).Stop();
    }

    [Test]
    public void Feel_initial_acceleration()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Speed = 27, IsRaceOn = true }
        };

        var accelerationIntensity = sut.SpeedIntensity;
        sut.Data = new WorldContext() { Speed = 31, IsRaceOn = true };

        sut.SpeedIntensity.Should().BeGreaterThan(accelerationIntensity);
    }

    [Test]
    public void Not_feel_initial_acceleration_if_gear_is_greater_than_two()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new Plugin(mock)
        {
            Data = new WorldContext() { Gear = "3", Speed = 27, IsRaceOn = true }
        };

        var accelerationIntensity = sut.SpeedIntensity;
        sut.Data = new WorldContext() { Gear = "3", Speed = 31, IsRaceOn = true };

        sut.SpeedIntensity.Should().BeGreaterThan(accelerationIntensity);
    }
}