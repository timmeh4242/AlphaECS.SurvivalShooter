using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;

namespace AlphaECS.SurvivalShooter
{
	public class DamageSystem : SystemBehaviour 
	{
		public override void Setup ()
		{
			var group = GroupFactory
				.AddTypes (new Type[] { typeof(EntityBehaviour), typeof(HealthComponent), typeof(InputComponent) })
				.WithPredicate((e) =>
				{
					var isHealthy = e.GetComponent<HealthComponent>().CurrentHealth.DistinctUntilChanged().Select(health => health >= 50).ToReactiveProperty();
					isHealthy.DistinctUntilChanged().StartWith(isHealthy.Value).Subscribe(x => Debug.Log("e " + e.Id + " is healthy " + x));
					return isHealthy;
				})
				.Create ();

			group.OnAdd().Subscribe (entity =>
			{
				var entityBehaviour = entity.GetComponent<EntityBehaviour> ();
				var health = entity.GetComponent<HealthComponent> ();
				health.CurrentHealth.Value = health.StartingHealth;

				health.CurrentHealth.DistinctUntilChanged ().Where (value => value <= 0).Subscribe (_ =>
				{
					health.IsDead.Value = true;
				}).AddTo (health);

				health.CurrentHealth.DistinctUntilChanged ().Where (value => value > 0).Subscribe (_ =>
				{
					health.IsDead.Value = false;
				}).AddTo (health);

				health.IsDead.DistinctUntilChanged ().Where (value => value == true).Subscribe (_ =>
				{
					Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_2 =>
					{
						PoolManager.GetPool ().RemoveEntity (entity);
						GameObject.Destroy (entityBehaviour.gameObject);
					}).AddTo(entityBehaviour);
				}).AddTo (health);
			}).AddTo (group);
				
			EventSystem.OnEvent<DamageEvent> ().Subscribe (_ =>
			{
				var targetHealth = _.Target.GetComponent<HealthComponent>();
				if(targetHealth.CurrentHealth.Value <= 0)
					return;

				targetHealth.CurrentHealth.Value -= _.DamageAmount;

				if(targetHealth.CurrentHealth.Value <= 0)
				{
					EventSystem.Publish (new DeathEvent (_.Attacker, _.Target));
				}
			}).AddTo (this);
		}
	}
}
