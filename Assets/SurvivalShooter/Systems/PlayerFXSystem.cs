using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using EcsRx.Groups;
using UniRx;
using Zenject;
using UnityEngine.UI;
using DG.Tweening;

namespace EcsRx.SurvivalShooter
{
	public class PlayerFXSystem : ReactiveSystemBehaviour
	{
		public Slider HealthSlider;
		public Image DamageImage;
		public float FlashSpeed = 5f;
		public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);
		Animator animator;

		void Awake()
		{
			Debug.Log ("awake");
		}

		void Start()
		{
			Debug.Log ("start");
		}
			
		public override void Setup ()
		{
			base.Setup ();

			animator = GetComponent <Animator> ();

			Debug.Log ("setup");
//			var entityView = GetComponent<EntityBehaviour> ();
//			var health = entityView.Entity.GetComponent<HealthComponent> ();
//			var previousValue = health.CurrentHealth.Value;
//			health.CurrentHealth.DistinctUntilChanged ().Subscribe (value =>
//			{
//				if (value >= previousValue)
//					return;
//
//				if(value > 0)
//				{
//					DamageImage.color = FlashColor;
//					DOTween.To (() => DamageImage.color, x => DamageImage.color = x, Color.clear, FlashSpeed);
//
//					HealthSlider.value = value;
//					var audioSources = GetComponentsInChildren<AudioSource>();
//					var soundFX = audioSources.Where(audioSource => audioSource.clip.name.Contains("Hurt")).FirstOrDefault();
//					soundFX.Play();				
//				}
//				else
//				{
//					animator.SetTrigger ("Die");
//					var audioSources = GetComponentsInChildren<AudioSource>();
//					var soundFX = audioSources.Where(audioSource => audioSource.clip.name.Contains("Death")).FirstOrDefault();
//					soundFX.Play();	
//				}
//			}).AddTo (this).AddTo (gameObject);
		}

		public override System.Collections.IEnumerator SetupAsync ()
		{
			Debug.Log ("setup async");

			return base.SetupAsync ();
		}
	}
}
