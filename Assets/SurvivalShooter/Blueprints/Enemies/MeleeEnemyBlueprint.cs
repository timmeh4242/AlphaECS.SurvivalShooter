using System.Collections;
using System.Collections.Generic;
using AlphaECS.SurvivalShooter;
using UnityEngine;
using AlphaECS;

[CreateAssetMenu(menuName = "Blueprint/MeleeEnemy")]
public class MeleeEnemyBlueprint : BlueprintBase
{
    public HealthComponent Health;
    public MeleeComponent Melee;

    public override void Apply(IEntity entity)
    {
        base.Apply(entity);

        var health = Instantiate(Health);
        entity.AddComponent(health);

        var melee = Instantiate(Melee);
        entity.AddComponent(melee);
    }
}
