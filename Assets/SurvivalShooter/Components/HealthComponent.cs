using AlphaECS;
using AlphaECS.Unity;
using UniRx;
using System;

namespace AlphaECS.SurvivalShooter
{
	public class HealthComponent : IComponent, IDisposableContainer, IDisposable
	{
		public int StartingHealth { get; set; }
		public IntReactiveProperty CurrentHealth { get; set; }
		public bool IsDamaged { get; set; }
		public BoolReactiveProperty IsDead { get; set; }

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
