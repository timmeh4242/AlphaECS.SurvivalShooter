using UnityEngine;
using UniRx;
using AlphaECS.Unity;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class PlayerAnimationSystem : SystemBehaviour
	{
		private static int isWalkingHash = Animator.StringToHash("IsWalking");
		
		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			var group = GroupFactory.Create (new Type[] { typeof(InputComponent), typeof(ViewComponent), typeof(Animator) });

			group.OnAdd().Subscribe (entity =>
			{
				var input = entity.GetComponent<InputComponent> ();
				var horizontal = input.Horizontal.DistinctUntilChanged ();
				var vertical = input.Vertical.DistinctUntilChanged ();
				var animator = entity.GetComponent<Animator>();

				Observable.CombineLatest (horizontal, vertical, (h, v) =>
				{
					return h != 0f || v != 0f;
				}).ToReadOnlyReactiveProperty().DistinctUntilChanged().Subscribe(value =>
				{
					animator.SetBool(isWalkingHash, value);
				}).AddTo(input.Disposer);
			}).AddTo (Disposer);
		}
	}
}
