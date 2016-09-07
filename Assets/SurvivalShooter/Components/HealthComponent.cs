using EcsRx;
using EcsRx.Unity;
using UniRx;
using System;

namespace EcsRx.SurvivalShooter
{
	public class HealthComponent : IComponent, IDisposableContainer, IDisposable
	{
		public int StartingHealth { get; set; }
		public IntReactiveProperty CurrentHealth { get; set; }
		public bool IsDamaged { get; set; }
		public BoolReactiveProperty IsDead { get; set; }

		private CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			set { _disposer = value; }
		}

		public void Dispose ()
		{
			Disposer.Dispose ();
		}

//		public Slider healthSlider;
//		public Image damageImage;
//		public AudioClip deathClip;
//		public float flashSpeed = 5f;
//		public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
//
//		Animator anim;
//		AudioSource playerAudio;
//		PlayerMovement playerMovement;
//		PlayerShooting playerShooting;
//		bool isDead;
//		bool damaged;
	}
}
