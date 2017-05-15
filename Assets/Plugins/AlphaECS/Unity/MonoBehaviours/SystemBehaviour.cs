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

        void OnDestroy()
        {
            Dispose();
        }

        [Inject]
		public virtual void Setup(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
        {
			EventSystem = eventSystem;
			PoolManager = poolManager;
			GroupFactory = groupFactory;
        }

        public virtual void Dispose()
        {
            Disposer.Dispose();
        }
    }
}
