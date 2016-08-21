using UniRx;
using UnityEngine;
using EcsRx.Unity;
using Zenject;
using System;
using System.Collections;
using EcsRx;

namespace EcsRx.SurvivalShooter
{
	public class DamageSystem : ISystem 
	{
		public void Setup ()
		{
			throw new NotImplementedException ();
		}
		public IEnumerator SetupAsync ()
		{
			throw new NotImplementedException ();
		}
		public IEventSystem EventSystem {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
//		[Inject]
//		public IEventSystem EventSystem { get; private set; }
//		[Inject]
//		public IPoolManager PoolManager { get; private set; }
//		private CompositeDisposable Subscriptions = new CompositeDisposable();
//
//		public IGroup TargetGroup 
//		{
//			get 
//			{
//				return new GroupBuilder()
//					.WithComponent<HealthComponent>()
//					.Build();			
//			}
//		}
//
//		public void Setup (IEntity entity)
//		{
//			var view = entity.GetComponent<ViewComponent> ();
//			var health = entity.GetComponent<HealthComponent> ();
//			health.CurrentHealth = new IntReactiveProperty ();
//			health.CurrentHealth.Value = health.StartingHealth;
//			health.IsDead = new BoolReactiveProperty ();
//			
//			health.CurrentHealth.DistinctUntilChanged ().Where (value => value <= 0).Subscribe (_ =>
//			{
//				health.IsDead.Value = true;
//			}).AddTo(view.View);
//
//			health.CurrentHealth.DistinctUntilChanged ().Where (value => value > 0).Subscribe (_ =>
//			{
//				health.IsDead.Value = false;
//			}).AddTo(view.View);
//
//			health.IsDead.DistinctUntilChanged ().Where(value => value == true).Subscribe (_ =>
//			{
//				Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_2 =>
//				{
//					PoolManager.GetPool().RemoveEntity(entity);
//					GameObject.Destroy (view.View);
//				});
//			}).AddTo(view.View);
//		}
//
//		public void StartSystem (GroupAccessor group)
//		{
//			EventSystem.OnEvent<DamageEvent> ().Subscribe (_ =>
//			{
//				var targetHealth = _.Target.GetComponent<HealthComponent>();
//				if(targetHealth.CurrentHealth.Value <= 0)
//					return;
//				
//				targetHealth.CurrentHealth.Value -= _.DamageAmount;
//
//				if(targetHealth.CurrentHealth.Value <= 0)
//					EventSystem.Publish (new DeathEvent (_.Attacker, _.Target));
//			}).AddTo (Subscriptions);
//		}
//
//		public void StopSystem (GroupAccessor group)
//		{
//			Subscriptions.Dispose ();
//		}
	}
}
