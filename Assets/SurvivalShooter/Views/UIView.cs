using UnityEngine;
using System.Collections;
using Zenject;
using EcsRx.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using EcsRx.Events;

public class UIView : MonoBehaviour {

	public IEventSystem EventSystem { get; private set; }
	public Animator animator;

	[Inject]
	public void Initialize(IEventSystem eventSystem)
	{
		EventSystem = eventSystem;

		EventSystem.Receive<DeathEvent> ().Where (_ => _.Target.HasComponent<InputComponent> ()).Subscribe (_ =>
		{
			animator.SetTrigger("GameOver");
		}).AddTo (this);
	}
}
