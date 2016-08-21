using UniRx;
using UnityEngine;
using EcsRx.Unity;
using EcsRx;
using System;

namespace EcsRx.SurvivalShooter
{
	public class ScoringSystem
	{
//		public IEventSystem EventSystem { get; private set; }
//		private CompositeDisposable Subscriptions = new CompositeDisposable();
//		public IntReactiveProperty Score { get; private set; }
//
//		public ScoringSystem(IEventSystem eventSystem)
//		{
//			EventSystem = eventSystem;
//			Score = new IntReactiveProperty ();
//		}
//
//		public IGroup TargetGroup 
//		{
//			get 
//			{
//				return new GroupBuilder ()
//					.WithComponent<ViewComponent> ()
//					.WithComponent<MeleeComponent> ()
//					.WithPredicate(x => x.GetComponent<ViewComponent>().View.GetComponent<NavMeshAgent>() != null)
//					.Build ();
//			}
//		}
//
//		public void StartSystem (GroupAccessor group)
//		{
//			EventSystem.OnEvent<DeathEvent> ().Subscribe (_ =>
//			{
//				Score.Value ++;
//			}).AddTo(Subscriptions);
//		}
//
//		public void StopSystem (GroupAccessor group)
//		{
//			Subscriptions.Dispose ();
//		}
	}
}
