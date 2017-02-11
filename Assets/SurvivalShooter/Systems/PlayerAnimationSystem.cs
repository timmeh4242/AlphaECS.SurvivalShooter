using UnityEngine;
using System.Collections;
using Zenject;
using AlphaECS.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using AlphaECS;
using AlphaECS.Unity;
using DG.Tweening;
using System.Linq;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class PlayerAnimationSystem : SystemBehaviour
	{
		public override void Setup ()
		{
			base.Setup ();

			var group = GroupFactory.Create (new Type[] { typeof(InputComponent), typeof(EntityBehaviour), typeof(Animator) });

			group.Entities.ObserveAdd ().Select(x => x.Value).StartWith(group.Entities).Subscribe (entity =>
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
					animator.SetBool("IsWalking", value);
				}).AddTo(input.Disposer);
			}).AddTo (Disposer);
		}
	}
}
