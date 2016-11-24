using UniRx;
using UnityEngine;
using EcsRx.Unity;
using EcsRx;
using System;

namespace EcsRx.SurvivalShooter
{
	public class ScoringSystem : SystemBehaviour
	{
		public IntReactiveProperty Score { get; private set; }

		public override void Setup ()
		{
			base.Setup ();

			Score = new IntReactiveProperty ();

			EventSystem.OnEvent<DeathEvent> ().Where (_ => !_.Target.HasComponent<InputComponent> ()).Subscribe (_ =>
			{
				Score.Value++;
			}).AddTo (this);
		}
	}
}
