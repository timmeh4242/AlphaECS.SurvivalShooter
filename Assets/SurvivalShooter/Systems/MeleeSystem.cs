using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using UniRx.Triggers;
using System;

namespace EcsRx.SurvivalShooter
{
	public class MeleeSystem : ISetupSystem
	{
		public IEventSystem EventSystem { get; private set; }

		public MeleeSystem(IEventSystem eventSystem)
		{
			EventSystem = eventSystem;
		}

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
			attacker.TargetInRange = new BoolReactiveProperty ();
			var collider = view.View.GetComponent<Collider> ();

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
						return;
					
					if(targetView.Entity.HasComponent<HealthComponent>() == false)
						return;
					
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
						var attackPosition = attacker.Target.GetComponent<ViewComponent>().View.transform.position;
						EventSystem.Publish (new DamageEvent (entity, attacker.Target, attacker.Damage, attackPosition));
					});
				}
				else
				{
					if(attacker.Attack != null)
						attacker.Attack.Dispose();
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
