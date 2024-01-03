using AlphaECS.Unity;
using UniRx;

namespace AlphaECS.SurvivalShooter
{
	public class InputComponent : ComponentBase
	{
        public FloatReactiveProperty Horizontal = new FloatReactiveProperty();
		public FloatReactiveProperty Vertical = new FloatReactiveProperty();
	}
}
