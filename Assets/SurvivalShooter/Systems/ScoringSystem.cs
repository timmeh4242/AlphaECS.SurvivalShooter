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

//			var group = GroupFactory.Create (new Type[]{ typeof(ViewComponent), typeof(MeleeComponent), typeof(NavMeshAgent) });
			EventSystem.OnEvent<DeathEvent> ().Subscribe (_ =>
			{
				Score.Value++;
			}).AddTo (this);
		}
	}
}
