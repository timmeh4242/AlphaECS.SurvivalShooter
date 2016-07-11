using EcsRx.Systems;
using EcsRx.Events;
using EcsRx.Groups;
using EcsRx.Unity.Components;
using UnityEngine;
using UniRx;
using Zenject;
using EcsRx.Entities;
using System;
using EcsRx.Pools;

namespace EcsRx.SurvivalShooter
{
	public class EnemyFXSystem : IManualSystem, ISetupSystem
	{
		[Inject]
		public IEventSystem EventSystem { get; set; }
		[Inject]
		public IPoolManager PoolManager { get; private set; }
		CompositeDisposable Subscriptions = new CompositeDisposable ();

		public IGroup TargetGroup 
		{
			get 
			{
				return new GroupBuilder ()
					.WithComponent<ViewComponent> ()
					.WithComponent<HealthComponent> ()
					.WithPredicate(x => x.GetComponent<ViewComponent>().View.GetComponent<NavMeshAgent>() != null)
					.Build ();
			}
		}

		public void Setup (IEntity entity)
		{
			var viewComponent = entity.GetComponent<ViewComponent> ();
			var health = entity.GetComponent<HealthComponent> ();
			var view = viewComponent.View;
			health.IsDead.DistinctUntilChanged ().Where (value => value == true).Subscribe (_ =>
			{
				view.GetComponent<CapsuleCollider> ().isTrigger = true;
				view.GetComponent<Animator> ().SetTrigger ("Die");
				var audio = view.GetComponent<AudioSource> ();
				audio.Play ();

//				audio.clip = DeathClip;
				audio.Play ();

				view.GetComponent<Rigidbody> ().isKinematic = true;
				//					ScoreManager.score += scoreValue;

				Observable.Timer (TimeSpan.FromSeconds (1)).Subscribe (_2 =>
				{
					var sink = Observable.EveryUpdate ().Subscribe (_3 =>
					{
//						view.transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
						view.transform.Translate (-Vector3.up * 2.5f * Time.deltaTime);
					});

					Observable.Timer (TimeSpan.FromSeconds (2)).Subscribe (_3 =>
					{
						sink.Dispose ();
//						PoolManager.GetPool().RemoveEntity(_.Target);
						GameObject.Destroy (view);
					});
				});
			}).AddTo (view);
		}

		public void StartSystem (GroupAccessor group)
		{
			EventSystem.Receive<DamageEvent> ().Subscribe (_ =>
			{
				if(_.Target.GetComponent<HealthComponent>().IsDead.Value == true)
					return;

				// take damage
				var view = _.Target.GetComponent<ViewComponent>().View;
				var audio = view.GetComponent<AudioSource>();
				audio.Play();
				var particles = view.GetComponentInChildren <ParticleSystem> ();
				particles.transform.position = _.HitPoint;
				particles.Play();
			}).AddTo (Subscriptions);
		}

		public void StopSystem (global::EcsRx.Groups.GroupAccessor group)
		{
			Subscriptions.Dispose ();
		}
	}
}
