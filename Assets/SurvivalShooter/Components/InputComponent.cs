using EcsRx;
using UniRx;

namespace EcsRx.SurvivalShooter
{
	public class InputComponent
	{
		public FloatReactiveProperty Horizontal { get; set; }
		public FloatReactiveProperty Vertical { get; set; }
	}
}
