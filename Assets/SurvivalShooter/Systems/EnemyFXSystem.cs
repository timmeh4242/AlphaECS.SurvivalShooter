using UniRx;
using UnityEngine;
using EcsRx.Unity;
using Zenject;
using System;
using System.Collections;
using EcsRx;

namespace EcsRx.SurvivalShooter
{
	public class EnemyFXSystem
	{
		const float DeathSinkSpeed = 2.5f;

//		public IGroup TargetGroup 
//		{
//			get 
//			{
//				return new GroupBuilder ()
//					.WithComponent<ViewComponent> ()
//					.WithComponent<HealthComponent> ()
//					.WithPredicate(x => x.GetComponent<ViewComponent>().View.GetComponent<NavMeshAgent>() != null)
//					.Build ();
//			}
//		}

//		public void Setup (IEntity entity)
		public void Setup ()
		{
//			var viewComponent = entity.GetComponent<ViewComponent> ();
//			var health = entity.GetComponent<HealthComponent> ();
//			var view = viewComponent.View;
//
//			health.IsDead.DistinctUntilChanged ().Where (value => value == true).Subscribe (_ =>
//			{
//				view.GetComponent<CapsuleCollider> ().isTrigger = true;
//				view.GetComponent<Animator> ().SetTrigger ("Die");
//				var soundFX = view.GetComponentsInChildren<AudioSource> ();
//				var deathFX = soundFX.Where(_2 => _2.clip.name.Contains("Death")).FirstOrDefault();
//				deathFX.Play();
//
//				view.GetComponent<Rigidbody> ().isKinematic = true;
////				ScoreManager.score += scoreValue;
//
//				Observable.Timer (TimeSpan.FromSeconds (1)).Subscribe (_2 =>
//				{
//					var sink = Observable.EveryUpdate ().Subscribe (_3 =>
//					{
//						view.transform.Translate (-Vector3.up * DeathSinkSpeed * Time.deltaTime);
//					}).AddTo(view);
//
//					Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_3 =>
//					{
//						sink.Dispose ();
//					});
//				});
//			}).AddTo (view);
		}

		public IEnumerator SetupAsync ()
		{
			yield break;
		}

//		public void StartSystem (GroupAccessor group)
//		{
//			EventSystem.OnEvent<DamageEvent> ().Subscribe (_ =>
//			{
//				if(_.Target.GetComponent<HealthComponent>().IsDead.Value == true)
//					return;
//
//				var view = _.Target.GetComponent<ViewComponent>().View;
//				var soundFX = view.GetComponentsInChildren<AudioSource>();
//				var hurtFX = soundFX.Where(_2 => _2.clip.name.Contains("Hurt")).FirstOrDefault();
//				hurtFX.Play();
//
//				var particles = view.GetComponentInChildren <ParticleSystem> ();
//				particles.transform.position = _.HitPoint;
//				particles.Play();
//			}).AddTo (Subscriptions);
//		}
//
//		public void StopSystem (GroupAccessor group)
//		{
//			Subscriptions.Dispose ();
//		}
	}
}
