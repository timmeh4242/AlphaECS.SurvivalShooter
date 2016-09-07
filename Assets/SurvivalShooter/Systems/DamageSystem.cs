using UniRx;
using UnityEngine;
using EcsRx.Unity;
using Zenject;
using System;
using System.Collections;
using EcsRx;

namespace EcsRx.SurvivalShooter
{
	public class DamageSystem : SystemBehaviour 
	{
		public override void Setup ()
		{
			var group = GroupFactory.Create(new Type[] { typeof(HealthComponent) });
			group.Entities.ObserveAdd ().Select(x => x.Value).StartWith(group.Entities).Subscribe (entity =>
			{
				var view = entity.GetComponent<ViewComponent> ();
				var health = entity.GetComponent<HealthComponent> ();
				health.CurrentHealth = new IntReactiveProperty ();
				health.CurrentHealth.Value = health.StartingHealth;
				health.IsDead = new BoolReactiveProperty ();

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
						GameObject.Destroy (view.View);
					});
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
