using UniRx;
using UnityEngine;
using AlphaECS;
using AlphaECS.Unity;
using UniRx.Triggers;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class MeleeSystem : SystemBehaviour
	{
		public override void Setup (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Setup (eventSystem, poolManager, groupFactory);

			var group = GroupFactory.Create (new Type[]{ typeof(ViewComponent), typeof(MeleeComponent) });
			group.OnAdd().Subscribe (entity =>
			{
				var viewComponent = entity.GetComponent<ViewComponent> ();
				var targetTransform = viewComponent.Transforms[0];
				var attacker = entity.GetComponent<MeleeComponent> ();
				attacker.TargetInRange = new BoolReactiveProperty ();
				var collider = targetTransform.GetComponent<Collider> ();

				collider.OnTriggerEnterAsObservable ().Subscribe (_ =>
				{
					var targetView = _.GetComponent <EntityBehaviour> ();
					if (targetView != null)
					{
						if(targetView.Entity.HasComponent<InputComponent>() == false)
							return;

						if(targetView.Entity.HasComponent<HealthComponent>() == false)
							return;

						attacker.Target = targetView.Entity;
						attacker.TargetInRange.Value = true;
					}
				}).AddTo(collider);

				collider.OnTriggerExitAsObservable ().Subscribe (_ =>
				{
					var targetView = _.GetComponent <EntityBehaviour> ();
					if (targetView != null)
					{
						if(targetView.Entity.HasComponent<InputComponent>() == false)
						{ return; }

						if(targetView.Entity.HasComponent<HealthComponent>() == false)
						{ return; }

						attacker.Target = null;
						attacker.TargetInRange.Value = false;
					}
				}).AddTo(collider);

				attacker.TargetInRange.DistinctUntilChanged ().Subscribe (value =>
				{
					if(value == true)
					{
						// handle shooting
						var delay = TimeSpan.FromSeconds(0f);
						var interval = TimeSpan.FromSeconds(1f / attacker.AttacksPerSecond);
						attacker.Attack = Observable.Timer(delay, interval).Subscribe(_ =>
						{
							var attackPosition = attacker.Target.GetComponent<ViewComponent>().Transforms[0].position;
							EventSystem.Publish (new DamageEvent (entity, attacker.Target, attacker.Damage, attackPosition));
						}).AddTo(attacker.Target.GetComponent<ViewComponent>().Disposer);
					}
					else
					{
						if(attacker.Attack != null)
						{ attacker.Attack.Dispose(); }
					}
				}).AddTo (viewComponent.Disposer);
			}).AddTo (this);
		}
			

		public void Execute(IEntity entity)
		{
//	        timer += Time.deltaTime;
//
//	        if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
//	        {
//	            Attack ();
//	        }
//
//	        if(playerHealth.currentHealth <= 0)
//	        {
//	            anim.SetTrigger ("PlayerDead");
//	        }
		}

		void Attack ()
		{
//			timer = 0f;

//	        if(playerHealth.currentHealth > 0)
//	        {
//	            playerHealth.TakeDamage (attackDamage);
//	        }
		}
	}
}
