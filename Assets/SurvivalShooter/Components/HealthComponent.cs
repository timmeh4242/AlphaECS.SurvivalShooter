using EcsRx.Components;
using UniRx;

namespace EcsRx.SurvivalShooter
{
	public class HealthComponent : IComponent
	{
		public int StartingHealth { get; set; }
		public IntReactiveProperty CurrentHealth { get; set; }
		public bool IsDamaged { get; set; }
		public bool IsDead { get; set; }

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
