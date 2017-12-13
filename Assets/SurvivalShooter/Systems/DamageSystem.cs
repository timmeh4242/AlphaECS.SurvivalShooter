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
		[Inject]
		public DeadEntities DeadEntities { get; set; }

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			DeadEntities.OnAdd ().Subscribe (entity =>
			{
				var viewComponent = entity.GetComponent<ViewComponent>();
				Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_2 =>
				{
					PoolManager.GetPool ().RemoveEntity (entity);
					GameObject.Destroy (viewComponent.Transforms[0].gameObject);
				}).AddTo(viewComponent.Disposer);
			}).AddTo (this.Disposer);

			var group = GroupFactory.AddTypes (new Type[] { typeof(ViewComponent), typeof(HealthComponent) }).Create ();
			group.AddTo (this.Disposer);
				
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
