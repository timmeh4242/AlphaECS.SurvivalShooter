using UniRx;
using UnityEngine;
using EcsRx.Unity;
using Zenject;
using System;
using System.Collections;
using EcsRx;

namespace EcsRx.SurvivalShooter
{
	public class NavMeshMovementSystem
	{
		Transform Target;
		HealthComponent TargetHealth;

//		public IGroup TargetGroup
//		{
//			get 
//			{
//				return new GroupBuilder ()
//					.WithComponent<ViewComponent> ()
//					.WithComponent<HealthComponent> ()
//					.WithPredicate(x => x.GetComponent<ViewComponent>().View.GetComponent<NavMeshAgent>() != null)
//					.Build ();
//			}
//		}
//
//		public IObservable<GroupAccessor> ReactToGroup (GroupAccessor group)
//		{
//			return Observable.EveryUpdate().Select(x => @group);
//		}
//
//		public void Execute (IEntity entity)
//		{
//			var navMeshAgent = entity.GetComponent<ViewComponent> ().View.GetComponent<NavMeshAgent> ();
//			var health = entity.GetComponent<HealthComponent> ();
//
//			if (Target == null)
//			{
//				var go = GameObject.FindGameObjectWithTag ("Player");
//				if (go == null)
//					return;
//				
//				Target = go.transform;
//				if (Target == null)
//					return;
//				
//				TargetHealth = Target.GetComponent<EntityBehaviour> ().Entity.GetComponent<HealthComponent>();
//				if (TargetHealth == null)
//					return;
//			}
//
//			if(health.CurrentHealth.Value > 0 && TargetHealth.CurrentHealth.Value > 0)
//	        {
//				navMeshAgent.SetDestination (Target.position);
//	        }
//	        else
//	        {
//				navMeshAgent.enabled = false;
//	        }
//		}
	}
}
