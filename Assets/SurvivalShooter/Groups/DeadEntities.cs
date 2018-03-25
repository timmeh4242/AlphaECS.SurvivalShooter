using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlphaECS;
using System;
using AlphaECS.Unity;
using UniRx;

namespace AlphaECS.SurvivalShooter
{
	public class DeadEntities : Group
	{
		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager)
		{
			Components = new HashSet<Type> { typeof(HealthComponent) };

			Func<IEntity, ReactiveProperty<bool>> checkIsDead = (e) =>
			{
				var health = e.GetComponent<HealthComponent> ();
				health.CurrentHealth.Value = health.StartingHealth;

				var isDead = health.CurrentHealth.DistinctUntilChanged ().Select (value => value <= 0).ToReactiveProperty();
				return isDead;
			};

			Predicates.Add(checkIsDead);

			base.Initialize (eventSystem, poolManager);
		} 
	}
}
