using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using System;

namespace EcsRx.SurvivalShooter
{
	public class ShootingSystem : IReactToGroupSystem, ISetupSystem
	{
		public IEventSystem EventSystem { get; private set; }

		Ray ShotRay;
		RaycastHit ShotRaycastHit;
		int ShootableMask;

		public ShootingSystem(IEventSystem eventSystem)
		{
			EventSystem = eventSystem;
			ShootableMask = LayerMask.GetMask("Shootable");
		}

		public IGroup TargetGroup
		{
			get
			{
				return new GroupBuilder()
					.WithComponent<ViewComponent>()
					.WithComponent<ShooterComponent>()
					.Build();
			}
		}

		public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
		{
			return Observable.EveryUpdate().Select(x => @group);
		}

		public void Setup (IEntity entity)
		{
			var view = entity.GetComponent<ViewComponent> ();
			var shooter = entity.GetComponent<ShooterComponent> ();
			shooter.IsShooting = new BoolReactiveProperty ();

			var transform = view.View.transform.FindChild("GunBarrelEnd");
			var shotParticles = transform.GetComponent<ParticleSystem> ();
			var shotLine = transform.GetComponent <LineRenderer> ();
			var shotAudio = transform.GetComponent<AudioSource> ();
			var shotLight = transform.GetComponent<Light> ();

			shooter.IsShooting.DistinctUntilChanged ().Subscribe (value =>
			{
				if(value == true)
				{
					// handle shooting
					var delay = TimeSpan.FromSeconds(0f);
					var interval = TimeSpan.FromSeconds(1f / shooter.ShotsPerSecond);
					shooter.Shoot = Observable.Timer(delay, interval).Subscribe(_ =>
					{
						ShotRay.origin = transform.position;
						ShotRay.direction = transform.forward;

						if (Physics.Raycast (ShotRay, out ShotRaycastHit, shooter.Range, ShootableMask))
						{
							var targetView = ShotRaycastHit.collider.GetComponent <EntityView> ();
							if (targetView != null)
							{
								var targetHealth = targetView.Entity.GetComponent<HealthComponent> ();
								if (targetHealth != null)
								{
									EventSystem.Publish (new DamageEvent (entity, targetView.Entity, shooter.Damage, ShotRaycastHit.point));
								}
							}

							shotLine.SetPosition (1, ShotRaycastHit.point);
						} else
						{
							shotLine.SetPosition (1, ShotRay.origin + ShotRay.direction * shooter.Range);
						}

						// handle fx
						shotAudio.Play ();
						shotLight.enabled = true;
						shotParticles.Stop ();
						shotParticles.Play ();
						shotLine.enabled = true;
						shotLine.SetPosition (0, transform.position);

						// disable the shot and/or fx after some arbitrary duration
						var shotDuration = TimeSpan.FromSeconds((1f / shooter.ShotsPerSecond) / 2f);
						Observable.Timer(shotDuration).Subscribe(_2 => {
							shotLine.enabled = false;
							shotLight.enabled = false;	
						}).AddTo(view.View);
					});
				}
				else
				{
					if(shooter.Shoot != null)
						shooter.Shoot.Dispose();
				}
			}).AddTo (view.View);
		}

		public void Execute(IEntity entity)
		{
			var shooter = entity.GetComponent<ShooterComponent> ();
			if (Input.GetButton ("Fire1"))
			{
				shooter.IsShooting.Value = true;
			}
			else
			{
				shooter.IsShooting.Value = false;
			}
		}
	}
}
