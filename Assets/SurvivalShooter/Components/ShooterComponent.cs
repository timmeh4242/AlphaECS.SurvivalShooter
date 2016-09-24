using EcsRx;
using UniRx;
using System;

namespace EcsRx.SurvivalShooter
{
	public class ShooterComponent : IComponent, IDisposableContainer, IDisposable
	{
		public int Damage { get; set; }
		public float ShotsPerSecond { get; set; }
		public IDisposable Shoot { get; set; }
		public float Range { get; set; }

		public BoolReactiveProperty IsShooting { get; set; }

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
