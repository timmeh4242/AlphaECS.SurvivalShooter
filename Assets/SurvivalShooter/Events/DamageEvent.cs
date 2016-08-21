using UnityEngine;
using EcsRx;

namespace EcsRx.SurvivalShooter
{
    public class DamageEvent
    {
		public IEntity Attacker { get; set; }
		public IEntity Target { get; set; }
        public int DamageAmount { get; private set; }
        public Vector3 HitPoint { get; private set; }

		public DamageEvent(IEntity attacker, IEntity target, int damageAmount, Vector3 hitPoint)
        {
			Attacker = attacker;
			Target = target;
			DamageAmount = damageAmount;
			HitPoint = hitPoint;
        }
    }
}