using EcsRx.Components;

namespace EcsRx.SurvivalShooter
{
	public class InputComponent : IComponent
	{
		public float Horizontal { get; set; }
		public float Vertical { get; set; }
	}
}
