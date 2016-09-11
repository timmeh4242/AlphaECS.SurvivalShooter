using UnityEngine;
using EcsRx.Unity;
using UniRx;
using Zenject;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Collections;

namespace EcsRx.SurvivalShooter
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

			var group = GroupFactory.Create(new Type[] { typeof(ViewComponent), typeof(HealthComponent), typeof(InputComponent) });

			group.Entities.ObserveAdd ().Select(x => x.Value).StartWith(group.Entities).Subscribe (entity =>
			{
				var health = entity.GetComponent<HealthComponent> ();
				var previousValue = health.CurrentHealth.Value;

				var audioSources = entity.GetComponent<ViewComponent> ().View.GetComponentsInChildren<AudioSource>();

				var hurtSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Hurt")).FirstOrDefault();
				var deathSound = audioSources.Where(audioSource => audioSource.clip.name.Contains("Death")).FirstOrDefault();

				var animator = entity.GetComponent<ViewComponent> ().View.GetComponent<Animator>();

				health.CurrentHealth.DistinctUntilChanged ().Subscribe (value =>
				{
					if (value >= previousValue)
						return;

					if(value >= 0)
					{
						DamageImage.color = FlashColor;
						DOTween.To (() => DamageImage.color, x => DamageImage.color = x, Color.clear, FlashSpeed);
	
						HealthSlider.value = value;
						hurtSound.Play();				
					}
					else
					{
						animator.SetTrigger ("Die");
						deathSound.Play();	
					}
				}).AddTo (this).AddTo (gameObject);
			}).AddTo(this).AddTo(group);
		}
	}
}
