using UniRx;
using UnityEngine;
using AlphaECS;
using AlphaECS.Unity;
using System;
using UniRx.Triggers;
using System.Collections;

namespace AlphaECS.SurvivalShooter
{
	public class LevelSystem : SystemBehaviour
	{
		public override void Setup ()
		{
			base.Setup ();

			EventSystem.OnEvent<DeathEvent> ().Where (_ => _.Target.HasComponent<InputComponent> ()).Subscribe (_ =>
			{
				Observable.EveryUpdate().Subscribe(__ =>
				{
					if(Input.GetMouseButton(0))
					{
						EventSystem.Publish(new LoadSceneEvent(){ SceneName = "Level_01" });
						Disposer.Clear();
					}
				}).AddTo(Disposer).AddTo(this);
			}).AddTo(this);
		}
	}
}
