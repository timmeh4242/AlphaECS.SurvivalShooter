using EcsRx;
using UniRx;
using System;

namespace EcsRx.SurvivalShooter
{
	public class ShooterComponent : IComponent
	{
		public int Damage { get; set; }
		public float ShotsPerSecond { get; set; }
		public IDisposable Shoot { get; set; }
		public float Range { get; set; }

//		public float FXDuration { get; set; }
		public BoolReactiveProperty IsShooting { get; set; }
	}
}
