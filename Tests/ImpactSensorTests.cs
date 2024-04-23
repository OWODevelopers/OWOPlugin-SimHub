using FluentAssertions;
using NUnit.Framework;
using OWOPluginSimHub.Domain;

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