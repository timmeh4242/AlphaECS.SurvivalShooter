using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using System;

namespace EcsRx.SurvivalShooter
{
	public class ScoringSystem : IManualSystem
	{
		public IEventSystem EventSystem { get; private set; }
		private CompositeDisposable Subscriptions = new CompositeDisposable();
		public IntReactiveProperty Score { get; private set; }

		public ScoringSystem(IEventSystem eventSystem)
		{
			EventSystem = eventSystem;
			Score = new IntReactiveProperty ();
		}

		public IGroup TargetGroup 
		{
			get 
			{
				return new GroupBuilder ()
					.WithComponent<ViewComponent> ()
					.WithComponent<MeleeComponent> ()
					.WithPredicate(x => x.GetComponent<ViewComponent>().View.GetComponent<NavMeshAgent>() != null)
					.Build ();
			}
		}

		public void StartSystem (GroupAccessor group)
		{
			EventSystem.OnEvent<DeathEvent> ().Subscribe (_ =>
			{
				Score.Value ++;
			}).AddTo(Subscriptions);
		}

		public void StopSystem (GroupAccessor group)
		{
			Subscriptions.Dispose ();
		}
	}
}
