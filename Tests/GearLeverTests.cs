using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using OWOPluginSimHub.Application;
using OWOPluginSimHub.Domain;
using static WorldContextBuilder;

public class GearLeverTests
{
    [Test]
    public void Gear_is_shifting()
    {
        var sut = new GearLever(Substitute.For<HapticSystem>());
        
        sut.Update(DuringRace(gear: "1"));
        
        sut.IsShiftingGear.Should().BeTrue();
    }

    [Test]
    public void Can_not_shift_gear_if_already_shifting()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new GearLever(mock);
        
        sut.Update(DuringRace(gear:"1"));
        sut.Update(DuringRace(gear:"2"));

        mock.Received(1).Stop();
    }

    [Test]
    public async Task Feel_gear_shift_after_previous_end()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new GearLever(mock);

        sut.Update(DuringRace(gear:"1"));
        await Task.Delay(350);
        sut.Update(DuringRace(gear:"2"));

        mock.Received(2).Stop();
    }

    [Test]
    public async Task Update_gear_even_when_shifting()
    {
        var mock = Substitute.For<HapticSystem>();
        var sut = new GearLever(mock);

        sut.Update(DuringRace(gear:"1"));
        sut.Update(DuringRace(gear:"2"));
        await Task.Delay(350);
        sut.Update(DuringRace(gear:"2"));

        mock.Received(1).Stop();
    }
}