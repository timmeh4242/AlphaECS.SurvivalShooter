using AlphaECS;
using UniRx;
using System;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter
{
	public class InputComponent : ComponentBase
	{
        public FloatReactiveProperty Horizontal = new FloatReactiveProperty();
		public FloatReactiveProperty Vertical = new FloatReactiveProperty();
	}
}
