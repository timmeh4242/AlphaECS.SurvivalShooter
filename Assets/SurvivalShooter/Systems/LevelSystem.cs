using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using System;
using UniRx.Triggers;

namespace EcsRx.SurvivalShooter
{
	public class LevelSystem : IManualSystem
	{
		public IEventSystem EventSystem { get; private set; }
		private CompositeDisposable Subscriptions = new CompositeDisposable();

		public LevelSystem(IEventSystem eventSystem)
		{
			EventSystem = eventSystem;
		}

		public IGroup TargetGroup 
		{
			get 
			{
				return new GroupBuilder ()
					.WithComponent<ViewComponent> ()
					.Build ();
			}
		}

		public void StartSystem (GroupAccessor group)
		{
			EventSystem.Receive<DeathEvent> ().Subscribe (_ =>
			{
//				this.OnMouseDownAsObservable().Subscribe(x => 
//				{
//					Debug.Log("restarting level");
//				});
			}).AddTo(Subscriptions);
		}

		public void StopSystem (GroupAccessor group)
		{
			Subscriptions.Dispose ();
		}
	}
}
