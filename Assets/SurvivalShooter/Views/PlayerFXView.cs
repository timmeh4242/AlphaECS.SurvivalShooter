using UnityEngine;
using System.Collections;
using Zenject;
using EcsRx.SurvivalShooter;
using UnityEngine.UI;
using UniRx;
using EcsRx.Events;
using EcsRx.Unity.MonoBehaviours;
using DG.Tweening;
using System.Linq;

public class PlayerFXView : MonoBehaviour
{
	Animator animator;

	[Inject]
	public void Initialize(IEventSystem eventSystem)
	{
		animator = GetComponent <Animator> ();

		Observable.TimerFrame (15).Subscribe (_ =>
		{
			var entityView = GetComponent<EntityBehaviour> ();
			var input = entityView.Entity.GetComponent<InputComponent> ();
			var horizontal = input.Horizontal.DistinctUntilChanged ();
			var vertical = input.Vertical.DistinctUntilChanged ();
			horizontal.Concat (vertical).Subscribe (value =>
			{
				animator.SetBool("IsWalking", input.Horizontal.Value != 0f || input.Vertical.Value != 0f);
			}).AddTo (this).AddTo (gameObject);
		}).AddTo (this);
	}
}
