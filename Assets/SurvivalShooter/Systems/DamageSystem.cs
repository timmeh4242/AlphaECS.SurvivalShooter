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
	public class DamageSystem : ISetupSystem, IManualSystem 
	{
		[Inject]
		public IEventSystem EventSystem { get; private set; }
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
			var health = entity.GetComponent<HealthComponent> ();
			health.CurrentHealth = new IntReactiveProperty ();
			health.CurrentHealth.Value = health.StartingHealth;
			
			health.CurrentHealth.DistinctUntilChanged ().Where (value => value <= 0).Subscribe (_ =>
			{
				health.IsDead = true;
			});

			health.CurrentHealth.DistinctUntilChanged ().Where (value => value > 0).Subscribe (_ =>
			{
				health.IsDead = false;
			});
//			}).AddTo (health);
		}

		public void StartSystem (GroupAccessor group)
		{
			EventSystem.Receive<DamageEvent> ().Subscribe (_ =>
			{
				Debug.Log(_.Target.Id);
				var targetHealth = _.Target.GetComponent<HealthComponent>();
				if(targetHealth.CurrentHealth.Value <= 0)
					return;
				
				targetHealth.CurrentHealth.Value -= _.DamageAmount;
			}).AddTo (Subscriptions);
		}

		public void StopSystem (GroupAccessor group)
		{
			Subscriptions.Dispose ();
		}
	}
}
