using UnityEngine;
using System.Collections;
using Zenject;
using EcsRx.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using EcsRx;
using EcsRx.Unity;
using UniRx.Triggers;

public class GameOverScreen : ComponentBehaviour
{
	public Animator animator;

	public override void Setup ()
	{
		base.Setup ();

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
