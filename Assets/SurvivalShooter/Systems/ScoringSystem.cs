using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using AlphaECS;
using System;

namespace AlphaECS.SurvivalShooter
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
