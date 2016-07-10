using EcsRx.Entities;
using EcsRx.Groups;
using EcsRx.Systems;
using EcsRx.Unity.Components;
using UniRx;
using UnityEngine;
using Zenject;

namespace EcsRx.SurvivalShooter
{
	public class PlayerMovementSystem : IReactToGroupSystem
	{
		public readonly float MovementSpeed = 2.0f;
		public int FloorMask;

		public PlayerMovementSystem()
		{
			FloorMask = LayerMask.GetMask("Floor");
		}

		public IGroup TargetGroup
		{
			get
			{
				return new GroupBuilder()
					.WithComponent<ViewComponent>()
					.WithComponent<InputComponent>()
					.WithPredicate(x => x.GetComponent<ViewComponent>().View.GetComponent<Rigidbody>() != null)
					.Build();
			}
		}

		public IObservable<GroupAccessor> ReactToGroup(GroupAccessor @group)
		{
			return Observable.EveryFixedUpdate().Select(x => @group);
		}

		public void Execute(IEntity entity)
		{
			// execute movement
			var input = entity.GetComponent<InputComponent> ();
			input.Horizontal = Input.GetAxisRaw("Horizontal");
			input.Vertical = Input.GetAxisRaw("Vertical");

			var movement = Vector3.zero;
			movement.Set(input.Horizontal, 0f, input.Vertical);
			var speed = 6f;
			movement = movement.normalized * speed * Time.deltaTime;
			var rb = entity.GetComponent<ViewComponent> ().View.transform.GetComponent<Rigidbody>();
			rb.MovePosition(rb.transform.position + movement);


			// execute turning
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 1000f, FloorMask))
			{
				Vector3 playerToMouse = hit.point - rb.transform.position;
				playerToMouse.y = 0f;

				Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
				rb.MoveRotation(newRotation);
			}
		}
	}
}
