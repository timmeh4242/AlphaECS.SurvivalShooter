using UnityEngine;
using UniRx;
using Zenject;
using System.Collections;
using System;

namespace AlphaECS
{
	public abstract class System : ISystem, IDisposableContainer, IDisposable
	{		
		public IEventSystem EventSystem { get; set; }
		public IPoolManager PoolManager { get; set; }

		protected CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			private set { _disposer = value; }
		}
						
		public virtual void Setup (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			
		}

		public virtual void Dispose ()
		{
			Disposer.Dispose ();
		}
	}
}
