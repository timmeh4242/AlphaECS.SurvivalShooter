using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using Zenject;
using System;
using EcsRx.Pools;

namespace EcsRx.SurvivalShooter
{
	public class DamageSystem : ISetupSystem, IManualSystem 
	{
		[Inject]
		public IEventSystem EventSystem { get; private set; }
		[Inject]
		public IPoolManager PoolManager { get; private set; }
		private CompositeDisposable Subscriptions = new CompositeDisposable();

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
			var view = entity.GetComponent<ViewComponent> ();
			var health = entity.GetComponent<HealthComponent> ();
			health.CurrentHealth = new IntReactiveProperty ();
			health.CurrentHealth.Value = health.StartingHealth;
			health.IsDead = new BoolReactiveProperty ();
			
			health.CurrentHealth.DistinctUntilChanged ().Where (value => value <= 0).Subscribe (_ =>
			{
				health.IsDead.Value = true;
			}).AddTo(view.View);

			health.CurrentHealth.DistinctUntilChanged ().Where (value => value > 0).Subscribe (_ =>
			{
				health.IsDead.Value = false;
			}).AddTo(view.View);

			health.IsDead.DistinctUntilChanged ().Where(value => value == true).Subscribe (_ =>
			{
				Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_2 =>
				{
					PoolManager.GetPool().RemoveEntity(entity);
					GameObject.Destroy (view.View);
				});
			}).AddTo(view.View);
		}

		public void StartSystem (GroupAccessor group)
		{
			EventSystem.Receive<DamageEvent> ().Subscribe (_ =>
			{
				var targetHealth = _.Target.GetComponent<HealthComponent>();
				if(targetHealth.CurrentHealth.Value <= 0)
					return;
				
				targetHealth.CurrentHealth.Value -= _.DamageAmount;

				if(targetHealth.CurrentHealth.Value <= 0)
					EventSystem.Publish (new DeathEvent (_.Attacker, _.Target));
			}).AddTo (Subscriptions);
		}

		public void StopSystem (GroupAccessor group)
		{
			Subscriptions.Dispose ();
		}
	}
}
