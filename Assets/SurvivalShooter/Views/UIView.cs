using UnityEngine;
using System.Collections;
using Zenject;
using EcsRx.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using EcsRx.Events;
using UniRx.Triggers;

public class UIView : MonoBehaviour {

	public IEventSystem EventSystem { get; private set; }
	public Animator animator;

	[Inject]
	public void Initialize(IEventSystem eventSystem)
	{
		EventSystem = eventSystem;

		EventSystem.OnEvent<DeathEvent> ().Where (_ => _.Target.HasComponent<InputComponent> ()).Subscribe (_ =>
		{
			animator.SetTrigger("GameOver");
			this.OnMouseDownAsObservable().Subscribe(x => 
			{
				Debug.Log("restarting level");
			});
		}).AddTo (this);
	}
}
