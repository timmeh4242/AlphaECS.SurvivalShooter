using EcsRx.Components;
using UniRx;
using EcsRx.Entities;

namespace EcsRx.SurvivalShooter
{
	public class MeleeComponent : IComponent
	{
		public int AttacksPerSecond { get; set; }
		public int Damage { get; set; }
		public IEntity Target { get; set; }
		public BoolReactiveProperty TargetInRange { get; set; }
	}
}
