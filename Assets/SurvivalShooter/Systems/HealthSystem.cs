using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using Zenject;

namespace EcsRx.SurvivalShooter
{
	public class HealthSystem : ISetupSystem
	{
		public IGroup TargetGroup
		{
			get 
			{
				return new GroupBuilder()
					.WithComponent<HealthComponent>()
					.Build();			
			}
		}

		public void Setup (IEntity entity)
		{
			var health = entity.GetComponent<HealthComponent> ();
			health.CurrentHealth = new IntReactiveProperty ();
			health.CurrentHealth.Value = health.StartingHealth;
		}
	}
}
