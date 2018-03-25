using System.Collections;
using System.Collections.Generic;
using AlphaECS.SurvivalShooter;
using UnityEngine;
using AlphaECS;

[CreateAssetMenu(menuName = "Blueprint/Player")]
public class PlayerBlueprint : BlueprintBase
{
    public InputComponent Input = new InputComponent();
    public HealthComponent Health = new HealthComponent();
    public ShooterComponent Shooter = new ShooterComponent();

    public override void Apply(IEntity entity)
    {
        base.Apply(entity);

        var clone = Instantiate(this);
        entity.AddComponent(clone.Input);
        entity.AddComponent(clone.Health);
        entity.AddComponent(clone.Shooter);
    }
}
