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

		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			Score = new IntReactiveProperty ();

			EventSystem.OnEvent<DeathEvent> ().Where (_ => !_.Target.HasComponent<InputComponent> ()).Subscribe (_ =>
			{
				Score.Value++;
			}).AddTo (this);
		}
	}
}
