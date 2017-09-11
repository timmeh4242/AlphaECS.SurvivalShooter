using AlphaECS;
using UniRx;
using System;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter
{
	public class ShooterComponent : ComponentBase
	{
        public int Damage;
        public float ShotsPerSecond;
        public IDisposable Shoot;
        public float Range;

        public BoolReactiveProperty IsShooting = new BoolReactiveProperty();
	}
}
