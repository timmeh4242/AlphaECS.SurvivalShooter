using UnityEngine;
using AlphaECS;

namespace AlphaECS.SurvivalShooter
{
    public class DeathEvent
    {
		public IEntity Attacker { get; set; }
		public IEntity Target { get; set; }

		public DeathEvent(IEntity attacker, IEntity target)
        {
			Attacker = attacker;
			Target = target;
        }
    }
}