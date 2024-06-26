using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using OWOPluginSimHub.Domain;
using static OWOGame.Muscle;

public class MuscleExtensionTests
{
    [Test]
    public void Concat_TwoMuscles_Collection()
    {
        var accelerationMuscles = Back;
        var brakeMuscles = Front;
        var allMuscles = accelerationMuscles.Concat(brakeMuscles).ToList();

        allMuscles.Should().HaveCount(10).And.Contain(Dorsal_R).And.Contain(Pectoral_R);
    }

    [Test]
    public void Clamp()
    {
        OWOPluginSimHub.Domain.Math.Clamp(10, 0, 5).Should().Be(5);
        OWOPluginSimHub.Domain.Math.Clamp(-1, 0, 5).Should().Be(0);
        OWOPluginSimHub.Domain.Math.Clamp(3, 0, 5).Should().Be(3);
    }

    [Test]
    public void Include_arms_when_turning_based_on_direction()
    {
        new SteeringMusclesBuilder() { AccelerationX = 4f }
            .ApplyDirectionForce(All.Except(new[] { Arm_R, Arm_L }))
            .Should().HaveCount(9).And.Contain(x => x.id == Arm_R.id);

        new SteeringMusclesBuilder() { AccelerationX = -4f }
            .ApplyDirectionForce(All.Except(new[] { Arm_R, Arm_L }))
            .Should().HaveCount(9).And.Contain(x => x.id == Arm_L.id);
    }

    [Test]
    public void Contains()
    {
        All.Contains(Pectoral_L, Pectoral_R).Should().BeTrue();
        Back.Contains(Pectoral_L, Pectoral_R).Should().BeFalse();
    }

    [Test]
    public void Keep_same_muscles_when_no_steering()
    {
        new SteeringMusclesBuilder()
            .ApplyDirectionForce(All.Except(new[] { Arm_R, Arm_L }))
            .Should().HaveCount(8);
    }

    [Test]
    public void Apply_intensity_multiplier_ignores_non_present_muscles()
    {
        Back.ApplyIntensityMultiplier(1.3f, new[] { Pectoral_R })
            .Should().NotContain(x => x.id == Pectoral_L.id);
    }

    [Test]
    public void Apply_intensity_multiplier_over_muscles()
    {
        All.ApplyIntensityMultiplier(.5f, All)
            .Should().HaveCount(10).And.Contain(x => x.intensity == 50);
    }
}