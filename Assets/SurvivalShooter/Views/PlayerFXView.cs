using UnityEngine;
using System.Collections;
using Zenject;
using EcsRx.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using EcsRx.Events;
using EcsRx.Unity.MonoBehaviours;
using DG.Tweening;
using System.Linq;

public class PlayerFXView : MonoBehaviour
{
	public Slider HealthSlider;
	public Image DamageImage;
	public float FlashSpeed = 5f;
	public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);

	Animator animator;

	[Inject]
	public void Initialize(IEventSystem eventSystem)
	{
		animator = GetComponent <Animator> ();

		Observable.TimerFrame (15).Subscribe (_ =>
		{
			var entityView = GetComponent<EntityView> ();
			var health = entityView.Entity.GetComponent<HealthComponent> ();
			var previousValue = health.CurrentHealth.Value;
			health.CurrentHealth.DistinctUntilChanged ().Subscribe (value =>
			{
				if (value >= previousValue)
					return;

				if(value > 0)
				{
					DamageImage.color = FlashColor;
					DOTween.To (() => DamageImage.color, x => DamageImage.color = x, Color.clear, FlashSpeed);

					HealthSlider.value = value;
					var audioSources = GetComponentsInChildren<AudioSource>();
					var soundFX = audioSources.Where(audioSource => audioSource.clip.name.Contains("Hurt")).FirstOrDefault();
					soundFX.Play();				
				}
				else
				{
					animator.SetTrigger ("Die");
					var audioSources = GetComponentsInChildren<AudioSource>();
					var soundFX = audioSources.Where(audioSource => audioSource.clip.name.Contains("Death")).FirstOrDefault();
					soundFX.Play();	
				}
			}).AddTo (this).AddTo (gameObject);

			var input = entityView.Entity.GetComponent<InputComponent> ();
			var horizontal = input.Horizontal.DistinctUntilChanged ();
			var vertical = input.Vertical.DistinctUntilChanged ();
			horizontal.Concat (vertical).Subscribe (value =>
			{
				animator.SetBool("IsWalking", input.Horizontal.Value != 0f || input.Vertical.Value != 0f);
			}).AddTo (this).AddTo (gameObject);
		}).AddTo (this);
	}
}
