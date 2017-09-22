using AlphaECS;
using AlphaECS.Unity;
using UniRx;
using System;
using UnityEngine;

namespace AlphaECS.SurvivalShooter
{
    [Serializable]
    public class HealthComponent : ComponentBase
	{
        public int StartingHealth;
        public IntReactiveProperty CurrentHealth = new IntReactiveProperty();
        public bool IsDamaged;
	}
}
