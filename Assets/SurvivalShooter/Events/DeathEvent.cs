using UnityEngine;
using EcsRx.Entities;

namespace EcsRx.SurvivalShooter
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