using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter
{
	public class EnemyFXSystem : SystemBehaviour
	{
		const float DeathSinkSpeed = 2.5f;

		public override void Setup (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Setup (eventSystem, poolManager, groupFactory);

			EventSystem.OnEvent<DamageEvent> ().Where(_ => _.Target.HasComponent<InputComponent>() == false).Subscribe (_ =>
			{
				if(_.Target.GetComponent<HealthComponent>().CurrentHealth.Value <= 0)
				{ return; }

				var viewComponent = _.Target.GetComponent<ViewComponent>();
				var soundFX = viewComponent.Transforms[0].GetComponentsInChildren<AudioSource>();
				var hurtFX = soundFX.Where(_2 => _2.clip.name.Contains("Hurt")).FirstOrDefault();
				hurtFX.Play();

				var particles = viewComponent.Transforms[0].GetComponentInChildren <ParticleSystem> ();
				particles.transform.position = _.HitPoint;
				particles.Play();
			}).AddTo (this);

			var group = GroupFactory.Create (new Type[]{ typeof(ViewComponent), typeof(HealthComponent), typeof(NavMeshAgent), typeof(CapsuleCollider), typeof(Animator), typeof(Rigidbody) });
			group.OnAdd().Subscribe (entity =>
			{
				var viewComponent = entity.GetComponent<ViewComponent> ();
				var targetTransform = viewComponent.Transforms[0];
				var healthComponent = entity.GetComponent<HealthComponent> ();
				var capsuleCollider = entity.GetComponent<CapsuleCollider>();
				var animator = entity.GetComponent<Animator>();
				var rb = entity.GetComponent<Rigidbody>();

				healthComponent.CurrentHealth.DistinctUntilChanged ().Where (value => value <= 0).Subscribe (_ =>
				{
					capsuleCollider.isTrigger = true;
					animator.SetTrigger ("Die");
					var soundFX = targetTransform.GetComponentsInChildren<AudioSource> ();
					var deathFX = soundFX.Where(_2 => _2.clip.name.Contains("Death")).FirstOrDefault();
					deathFX.Play();

					rb.isKinematic = true;
//					ScoreManager.score += scoreValue;

					Observable.Timer (TimeSpan.FromSeconds (1)).Subscribe (_2 =>
					{
						var sink = Observable.EveryUpdate ().Subscribe (_3 =>
						{
							targetTransform.Translate (-Vector3.up * DeathSinkSpeed * Time.deltaTime);
						}).AddTo(viewComponent.Disposer);

						Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_3 =>
						{
							sink.Dispose ();
						});
					});
				}).AddTo (viewComponent.Disposer);
			}).AddTo (this);
		}
	}
}
