using EcsRx.Components;
using UniRx;

namespace EcsRx.SurvivalShooter
{
	public class InputComponent : IComponent
	{
		public FloatReactiveProperty Horizontal { get; set; }
		public FloatReactiveProperty Vertical { get; set; }
	}
}
