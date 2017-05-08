using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using AlphaECS;
using UnityEngine.AI;

namespace AlphaECS.SurvivalShooter
{
	public class NavMeshMovementSystem : SystemBehaviour
	{
		Transform Target;
		HealthComponent TargetHealth;

		public override void Setup ()
		{
			base.Setup ();

			var group = GroupFactory.Create (new Type[] { typeof(HealthComponent), typeof(EntityBehaviour), typeof(UnityEngine.AI.NavMeshAgent) });
			group.OnAdd().Subscribe (entity =>
			{
				var entityBehaviour = entity.GetComponent<EntityBehaviour>();
				var navMeshAgent = entity.GetComponent<NavMeshAgent> ();
				var health = entity.GetComponent<HealthComponent> ();

				Observable.EveryUpdate ().Subscribe (_ =>
				{
					if (Target == null)
					{
						var go = GameObject.FindGameObjectWithTag ("Player");
						if (go == null)
							return;

						Target = go.transform;
						if (Target == null)
							return;

						TargetHealth = Target.GetComponent<EntityBehaviour> ().Entity.GetComponent<HealthComponent> ();
						if (TargetHealth == null)
							return;
					}

					if (health.CurrentHealth.Value > 0 && TargetHealth.CurrentHealth.Value > 0)
					{
						navMeshAgent.SetDestination (Target.position);
					} else
					{
						navMeshAgent.enabled = false;
					}
				}).AddTo (navMeshAgent).AddTo(health);
			}).AddTo (this);
		}
	}
}
