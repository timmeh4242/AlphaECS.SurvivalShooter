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

		public override void Setup ()
		{
			base.Setup ();

			EventSystem.OnEvent<DamageEvent> ().Where(_ => _.Target.HasComponent<InputComponent>() == false).Subscribe (_ =>
			{
				if(_.Target.GetComponent<HealthComponent>().IsDead.Value == true)
					return;

				var entityBehaviour = _.Target.GetComponent<EntityBehaviour>();
				var soundFX = entityBehaviour.GetComponentsInChildren<AudioSource>();
				var hurtFX = soundFX.Where(_2 => _2.clip.name.Contains("Hurt")).FirstOrDefault();
				hurtFX.Play();

				var particles = entityBehaviour.GetComponentInChildren <ParticleSystem> ();
				particles.transform.position = _.HitPoint;
				particles.Play();
			}).AddTo (this);

			var group = GroupFactory.Create (new Type[]{ typeof(EntityBehaviour), typeof(HealthComponent), typeof(NavMeshAgent), typeof(CapsuleCollider), typeof(Animator), typeof(Rigidbody) });
			group.OnAdd().Subscribe (entity =>
			{
				var entityBehaviour = entity.GetComponent<EntityBehaviour> ();
				var healthComponent = entity.GetComponent<HealthComponent> ();
				var capsuleCollider = entity.GetComponent<CapsuleCollider>();
				var animator = entity.GetComponent<Animator>();
				var rb = entity.GetComponent<Rigidbody>();

				healthComponent.IsDead.DistinctUntilChanged ().Where (value => value == true).Subscribe (_ =>
				{
					capsuleCollider.isTrigger = true;
					animator.SetTrigger ("Die");
					var soundFX = entityBehaviour.GetComponentsInChildren<AudioSource> ();
					var deathFX = soundFX.Where(_2 => _2.clip.name.Contains("Death")).FirstOrDefault();
					deathFX.Play();

					rb.isKinematic = true;
//					ScoreManager.score += scoreValue;

					Observable.Timer (TimeSpan.FromSeconds (1)).Subscribe (_2 =>
					{
						var sink = Observable.EveryUpdate ().Subscribe (_3 =>
						{
							entityBehaviour.transform.Translate (-Vector3.up * DeathSinkSpeed * Time.deltaTime);
						}).AddTo(entityBehaviour.Disposer);

						Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_3 =>
						{
							sink.Dispose ();
						});
					});
				}).AddTo (entityBehaviour.Disposer);
			}).AddTo (this);
		}
	}
}
