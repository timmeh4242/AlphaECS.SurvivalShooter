using EcsRx;
using UniRx;
using System;

namespace EcsRx.SurvivalShooter
{
	public class InputComponent : IComponent, IDisposableContainer, IDisposable
	{
		public FloatReactiveProperty Horizontal { get; set; }
		public FloatReactiveProperty Vertical { get; set; }

		private CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			set { _disposer = value; }
		}

		public void Dispose ()
		{
			Disposer.Dispose ();
		}
	}
}
