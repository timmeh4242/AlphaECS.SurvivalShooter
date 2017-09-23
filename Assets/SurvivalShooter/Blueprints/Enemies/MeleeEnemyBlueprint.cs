using System.Collections;
using System.Collections.Generic;
using AlphaECS.SurvivalShooter;
using UnityEngine;
using AlphaECS;

[CreateAssetMenu(menuName = "Blueprint/MeleeEnemy")]
public class MeleeEnemyBlueprint : BlueprintBase
{
    public HealthComponent Health = new HealthComponent();
    public MeleeComponent Melee = new MeleeComponent();

    public override void Apply(IEntity entity)
    {
        base.Apply(entity);

        var clone = Instantiate(this);
        //var health = Instantiate(Health);
        entity.AddComponent(clone.Health);

        //var melee = Instantiate(Melee);
        entity.AddComponent(clone.Melee);
    }
}
