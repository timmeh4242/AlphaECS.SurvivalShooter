using UniRx;
using UnityEngine;
using EcsRx.Unity;
using EcsRx;
using System;
using Zenject;

namespace EcsRx.SurvivalShooter
{
	public class ShootingSystem : SystemBehaviour
	{
		Ray ShotRay;
		RaycastHit ShotRaycastHit;
		int ShootableMask;
			
		public override void Setup ()
		{
			ShootableMask = LayerMask.GetMask("Shootable");

			var group = new Group (typeof(ViewComponent), typeof(ShooterComponent));
			group.Entities.ObserveAdd ().Subscribe (e =>
			{
				var entity = e.Value;
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
								var targetView = ShotRaycastHit.collider.GetComponent <EntityBehaviour> ();
								if (targetView != null)
								{
									var targetHealth = targetView.Entity.GetComponent<HealthComponent> ();
									if (targetHealth != null)
									{
										EventSystem.Publish (new DamageEvent (entity, targetView.Entity, shooter.Damage, ShotRaycastHit.point));
									}
								}

								shotLine.SetPosition (1, ShotRaycastHit.point);
							}
							else
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
				}).AddTo(view.View);	
			}).AddTo (this);

			Observable.EveryUpdate().Subscribe(_ =>
			{
				foreach(var entity in group.Entities)
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
			}).AddTo(this);

			Container.Inject(group);
		}
	}
}
