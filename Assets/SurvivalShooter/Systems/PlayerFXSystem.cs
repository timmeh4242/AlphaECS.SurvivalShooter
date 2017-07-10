using UnityEngine;
using AlphaECS.Unity;
using UniRx;
using Zenject;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace AlphaECS.SurvivalShooter
{
	public class PlayerFXSystem : SystemBehaviour
	{
		public Slider HealthSlider;
		public Image DamageImage;
		public float FlashSpeed = 5f;
		public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);

		[Inject]
		public DeadEntities DeadEntities { get; set; }
			
		public override void Setup (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Setup (eventSystem, poolManager, groupFactory);

			var group = GroupFactory.Create(new Type[] { typeof(ViewComponent), typeof(HealthComponent), typeof(InputComponent), typeof(Animator) });
			group.OnAdd().Subscribe (entity =>
			{
				var viewComponent = entity.GetComponent<ViewComponent>();
				var targetTransform = viewComponent.Transforms[0];
				var health = entity.GetComponent<HealthComponent> ();
				var previousValue = health.CurrentHealth.Value;

				var audioSources = targetTransform.GetComponentsInChildren<AudioSource>();
				var hurtSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Hurt")).FirstOrDefault();

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

					HealthSlider.value = value;
					previousValue = value;
				}).AddTo(this.Disposer).AddTo(health.Disposer);
			}).AddTo(this.Disposer);

			DeadEntities.OnAdd ().Subscribe (entity =>
			{
				if(entity.HasComponent<InputComponent>() == false)
					return;
				
				var viewComponent = entity.GetComponent<ViewComponent>();
				var animator = entity.GetComponent<Animator>();

				var audioSources = viewComponent.Transforms[0].GetComponentsInChildren<AudioSource>();
				var deathSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Death")).FirstOrDefault();
				animator.SetTrigger ("Die");
				deathSound.Play();

			}).AddTo (this.Disposer);
		}
	}
}
