using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using AlphaECS;

namespace AlphaECS.Unity
{
    public abstract class SystemBehaviour : MonoBehaviour, ISystem, IDisposableContainer, IDisposable
    {
		public IEventSystem EventSystem { get; set; }
		public IPoolManager PoolManager { get; set; }
		public GroupFactory GroupFactory { get; set; }

		[Inject]
        public PrefabFactory PrefabFactory { get; set; }

        private CompositeDisposable _disposer = new CompositeDisposable();
        public CompositeDisposable Disposer
        {
            get { return _disposer; }
            private set { _disposer = value; }
        }
			
        [Inject]
		public virtual void Initialize(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
        {
			EventSystem = eventSystem;
			PoolManager = poolManager;
			GroupFactory = groupFactory;
        }

		public virtual void OnEnable() { }

		public virtual void OnDisable()
		{
			Disposer.Clear ();
		}

		public virtual void OnDestroy()
		{
			if (EcsApplication.IsQuitting) { return; }
			Dispose();
		}

        public virtual void Dispose()
        {
            Disposer.Dispose();
        }

		public virtual void OnApplicationQuit()
		{
			EcsApplication.IsQuitting = true;
		}
    }
}
