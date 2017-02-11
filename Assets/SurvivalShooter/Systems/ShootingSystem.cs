using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using AlphaECS;
using System;
using Zenject;

namespace AlphaECS.SurvivalShooter
{
	public class ShootingSystem : SystemBehaviour
	{
		Ray ShotRay;
		RaycastHit ShotRaycastHit;
		int ShootableMask;
			
		public override void Setup ()
		{
			ShootableMask = LayerMask.GetMask("Shootable");

			var group = GroupFactory.Create(new Type[] { typeof(EntityBehaviour), typeof(ShooterComponent) });

			group.Entities.ObserveAdd ().Select(x => x.Value).StartWith(group.Entities).Subscribe (entity =>
			{
				var entityBehaviour = entity.GetComponent<EntityBehaviour> ();
				var shooter = entity.GetComponent<ShooterComponent> ();
				shooter.IsShooting = new BoolReactiveProperty ();

				var gunBarrel = entityBehaviour.transform.FindChild("GunBarrelEnd");
				var shotParticles = gunBarrel.GetComponent<ParticleSystem> ();
				var shotLine = gunBarrel.GetComponent <LineRenderer> ();
				var shotAudio = gunBarrel.GetComponent<AudioSource> ();
				var shotLight = gunBarrel.GetComponent<Light> ();

				shooter.IsShooting.DistinctUntilChanged ().Subscribe (value =>
				{
					if(value == true)
					{
						// handle shooting
						var delay = TimeSpan.FromSeconds(0f);
						var interval = TimeSpan.FromSeconds(1f / shooter.ShotsPerSecond);
						shooter.Shoot = Observable.Timer(delay, interval).Subscribe(_ =>
						{
							ShotRay.origin = gunBarrel.position;
							ShotRay.direction = gunBarrel.forward;

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
							shotLine.SetPosition (0, gunBarrel.position);

							// disable the shot and/or fx after some arbitrary duration
							var shotDuration = TimeSpan.FromSeconds((1f / shooter.ShotsPerSecond) / 2f);
							Observable.Timer(shotDuration).Subscribe(_2 =>
							{
								shotLine.enabled = false;
								shotLight.enabled = false;	
							}).AddTo(this.Disposer).AddTo(entityBehaviour.Disposer);
						}).AddTo(this.Disposer).AddTo(shooter.Disposer);
					}
					else
					{
						if(shooter.Shoot != null)
							shooter.Shoot.Dispose();
					}
				}).AddTo(this.Disposer).AddTo(shooter.Disposer);	
			}).AddTo (this.Disposer);

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
			}).AddTo(this.Disposer);
		}
	}
}
