using UnityEngine;
using System.Collections;
using Zenject;
using AlphaECS.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using AlphaECS;
using AlphaECS.Unity;
using UniRx.Triggers;

public class GameOverScreen : ComponentBehaviour
{
	public Animator animator;

	public override void Setup (IEventSystem eventSystem)
	{
		base.Setup (eventSystem);

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
