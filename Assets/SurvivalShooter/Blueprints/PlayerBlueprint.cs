using System.Collections;
using System.Collections.Generic;
using AlphaECS.SurvivalShooter;
using UnityEngine;
using AlphaECS;

[CreateAssetMenu(menuName = "Blueprint/Player")]
public class PlayerBlueprint : BlueprintBase
{
    public HealthComponent Health;

	public GameObject Target;

    public override void Apply(IEntity entity)
    {
        base.Apply(entity);

        entity.AddComponent(Health);
    }

    [ExecuteInEditMode]
    private void OnEnable()
    {
        if(Health == null)
        {
            Health = ScriptableObject.CreateInstance<HealthComponent>();
        }
    }
}
