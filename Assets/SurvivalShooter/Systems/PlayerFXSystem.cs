using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using EcsRx.Groups;
using UniRx;
using Zenject;

namespace EcsRx.SurvivalShooter
{
	public class PlayerFXSystem : IManualSystem
	{
		[Inject]
		public IEventSystem EventSystem { get; private set; }
		CompositeDisposable Subscriptions = new CompositeDisposable ();

		public IGroup TargetGroup 
		{
			get 
			{
				return new GroupBuilder ()
					.WithComponent<ViewComponent> ()
					.WithComponent<HealthComponent> ()
					.WithComponent<InputComponent> ()
					.Build ();
			}
		}

		public void StartSystem (GroupAccessor group)
		{
			EventSystem.Receive<DamageEvent> ().Subscribe (_ =>
			{

			}).AddTo (Subscriptions);
		}

		public void StopSystem (GroupAccessor group)
		{
			Subscriptions.Dispose ();
		}
	}
}
