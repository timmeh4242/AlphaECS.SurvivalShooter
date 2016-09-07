using UniRx;
using UnityEngine;
using EcsRx.Unity;
using Zenject;
using System;
using System.Collections;
using EcsRx;

namespace EcsRx.SurvivalShooter
{
	public class NavMeshMovementSystem : SystemBehaviour
	{
		Transform Target;
		HealthComponent TargetHealth;

		public override void Setup ()
		{
			base.Setup ();

			var group = GroupFactory.Create (new Type[] { typeof(HealthComponent), typeof(ViewComponent), typeof(NavMeshAgent) });
			group.Entities.ObserveAdd ().Select (x => x.Value).StartWith (group.Entities).Subscribe (entity =>
			{
				var navMeshAgent = entity.GetComponent<ViewComponent> ().View.GetComponent<NavMeshAgent> ();
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
