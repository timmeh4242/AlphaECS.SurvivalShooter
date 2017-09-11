using AlphaECS;
using UniRx;
using System;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter
{
	public class MeleeComponent : ComponentBase
	{
        public int Damage;
        public float AttacksPerSecond;
        public IEntity Target;
        public BoolReactiveProperty TargetInRange = new BoolReactiveProperty();
        public IDisposable Attack;
	}
}
