using AlphaECS;
using AlphaECS.Unity;
using UniRx;
using System;
using UnityEngine;
using System.Collections.Generic;

namespace AlphaECS.Unity
{
	public class ViewComponent : IComponent, IDisposable, IDisposableContainer
	{
		public ReactiveCollection<Transform> Transforms = new ReactiveCollection<Transform>();

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

		public ViewComponent() { }
	}
}
