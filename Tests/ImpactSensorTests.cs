using System.Threading.Tasks;
using FluentAssertions;
using OWOPlugin;
using NSubstitute;
using NUnit.Framework;

public class ImpactSensorTests
{
    [Test]
    public void Normal_impact()
    {
        var sut = new ImpactSensor();
        sut.Update(80);
        sut.Update(1);
        sut.Intensity().Should().Be(80);
    }

    [Test]
    public void Strong_impact()
    {
        var sut = new ImpactSensor();
        sut.Update(101);
        sut.Update(1);
        sut.Intensity().Should().Be(100);
    }

    [Test]
    public void Cinematic_impact()
    {
        var sut = new ImpactSensor();
        sut.Update(101);
        sut.Update(0);
        sut.Intensity().Should().Be(0);
    }

    [Test]
    public void No_impact()
    {
        var sut = new ImpactSensor();
        sut.Update(0);
        sut.Update(0);
        sut.Intensity().Should().Be(0);
    }

    [Test]
    public void Feel_no_impact_when_increasing_speed()
    {
        var sut = new ImpactSensor();
        sut.Update(0);
        sut.Update(100);
        sut.Intensity().Should().Be(0);
    }
}

public class GearLeverTests
{
    [Test]
    public void Gear_is_shifting()
    {
        var sut = new GearLever(Substitute.For<HapticSystem>());
        sut.Update(new WorldContext() { Gear = "1" });
        sut.IsShiftingGear.Should().BeTrue();
    }

    [Test]
    public void Can_not_shift_gear_if_already_shifting()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new GearLever(mock);
        sut.Update(new WorldContext() { Gear = "1" });
        sut.Update(new WorldContext() { Gear = "2" });

        mock.Received(1).Stop();
    }

    [Test]
    public async Task Feel_gear_shift_after_previous_end()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new GearLever(mock);

        sut.Update(new WorldContext() { Gear = "1" });
        await Task.Delay(350);
        sut.Update(new WorldContext() { Gear = "2" });

        mock.Received(2).Stop();
    }

    [Test]
    public async Task Update_gear_even_when_shifting()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new GearLever(mock);

        sut.Update(new WorldContext() { Gear = "1" });
        sut.Update(new WorldContext() { Gear = "2" });
        await Task.Delay(350);
        sut.Update(new WorldContext() { Gear = "2" });

        mock.Received(1).Stop();
    }
}