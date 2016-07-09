using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using UniRx.Triggers;

namespace EcsRx.SurvivalShooter
{
	public class MeleeSystem : ISetupSystem
	{
//		public IEventSystem EventSystem { get; private set; }

//		public MeleeSystem(EventSystem eventSystem)
//		{
//			EventSystem = eventSystem;
//		}

		public IGroup TargetGroup
		{
			get
			{
				return new GroupBuilder()
					.WithComponent<ViewComponent>()
					.WithComponent<MeleeComponent>()
					.Build();
			}
		}

		public void Setup (IEntity entity)
		{
			var view = entity.GetComponent<ViewComponent> ();
			var attacker = entity.GetComponent<MeleeComponent> ();
			var collider = view.View.GetComponent<Collider> ();


			collider.OnTriggerEnterAsObservable ().Subscribe (_ =>
			{
				var targetView = _.GetComponent <EntityView> ();
				if (targetView != null)
				{
					var targetHealth = targetView.Entity.GetComponent<HealthComponent> ();
					if (targetHealth != null)
					{
						attacker.Target = targetView.Entity;
						attacker.TargetInRange.Value = true;
					}
				}
			}).AddTo(collider);

			collider.OnTriggerExitAsObservable ().Subscribe (_ =>
			{
				var targetView = _.GetComponent <EntityView> ();
				if (targetView != null)
				{
					var targetHealth = targetView.Entity.GetComponent<HealthComponent> ();
					if (targetHealth != null)
					{
						attacker.Target = null;
						attacker.TargetInRange.Value = false;
					}
				}
			}).AddTo(collider);

			attacker.TargetInRange.DistinctUntilChanged ().Subscribe (_ =>
			{
				if(_ == true)
				{
					var attackPosition = attacker.Target.GetComponent<ViewComponent>().View.transform.position;
//					EventSystem.Publish (new DamageEvent (entity, attacker.Target, attacker.Damage, attackPosition));

					Debug.Log("target in range");
				}
				else
				{
					Debug.Log("target out of range");
				}
			}).AddTo (view.View);
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
