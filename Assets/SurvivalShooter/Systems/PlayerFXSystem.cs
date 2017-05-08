using UnityEngine;
using AlphaECS.Unity;
using UniRx;
using Zenject;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Collections;

namespace AlphaECS.SurvivalShooter
{
	public class PlayerFXSystem : SystemBehaviour
	{
		public Slider HealthSlider;
		public Image DamageImage;
		public float FlashSpeed = 5f;
		public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);
			
		public override void Setup ()
		{
			base.Setup ();

			var group = GroupFactory.Create(new Type[] { typeof(EntityBehaviour), typeof(HealthComponent), typeof(InputComponent), typeof(Animator) });

			group.OnAdd().Subscribe (entity =>
			{
				var entityBehaviour = entity.GetComponent<EntityBehaviour>();
				var health = entity.GetComponent<HealthComponent> ();
				var previousValue = health.CurrentHealth.Value;

				var audioSources = entityBehaviour.GetComponentsInChildren<AudioSource>();

				var hurtSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Hurt")).FirstOrDefault();
				var deathSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Death")).FirstOrDefault();

				var animator = entity.GetComponent<Animator>();

				health.CurrentHealth.DistinctUntilChanged ().Subscribe (value =>
				{
					// if you got hurt and are still alive...
					if(value < previousValue && value >= 0)
					{
						if(DamageImage != null)
						{
							DamageImage.color = FlashColor;
							DOTween.To (() => DamageImage.color, x => DamageImage.color = x, Color.clear, FlashSpeed);
						}
	
						hurtSound.Play();				
					}
					// if you gained health...
					else if(value >= previousValue && value >= 0)
					{

					}
					// if you're dead...
					else if(value < 0)
					{
						animator.SetTrigger ("Die");
						deathSound.Play();
					}

					HealthSlider.value = value;
					previousValue = value;
				}).AddTo(this.Disposer).AddTo(health.Disposer);
			}).AddTo(this.Disposer);
		}
	}
}
