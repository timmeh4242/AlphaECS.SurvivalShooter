using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using System.Linq;

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

				var view = _.Target.GetComponent<ViewComponent>().View;
				var soundFX = view.GetComponentsInChildren<AudioSource>();
				var hurtFX = soundFX.Where(_2 => _2.clip.name.Contains("Hurt")).FirstOrDefault();
				hurtFX.Play();

				var particles = view.GetComponentInChildren <ParticleSystem> ();
				particles.transform.position = _.HitPoint;
				particles.Play();
			}).AddTo (this);

			var group = GroupFactory.Create (new Type[]{ typeof(ViewComponent), typeof(HealthComponent), typeof(NavMeshAgent) });
			group.Entities.ObserveAdd ().Select (x => x.Value).StartWith (group.Entities).Subscribe (entity =>
			{
				var viewComponent = entity.GetComponent<ViewComponent> ();
				var health = entity.GetComponent<HealthComponent> ();
				var view = viewComponent.View;

				health.IsDead.DistinctUntilChanged ().Where (value => value == true).Subscribe (_ =>
				{
					view.GetComponent<CapsuleCollider> ().isTrigger = true;
					view.GetComponent<Animator> ().SetTrigger ("Die");
					var soundFX = view.GetComponentsInChildren<AudioSource> ();
					var deathFX = soundFX.Where(_2 => _2.clip.name.Contains("Death")).FirstOrDefault();
					deathFX.Play();

					view.GetComponent<Rigidbody> ().isKinematic = true;
//					ScoreManager.score += scoreValue;

					Observable.Timer (TimeSpan.FromSeconds (1)).Subscribe (_2 =>
					{
						var sink = Observable.EveryUpdate ().Subscribe (_3 =>
						{
							view.transform.Translate (-Vector3.up * DeathSinkSpeed * Time.deltaTime);
						}).AddTo(view);

						Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_3 =>
						{
							sink.Dispose ();
						});
					});
				}).AddTo (view);
			}).AddTo (this);
		}
	}
}
