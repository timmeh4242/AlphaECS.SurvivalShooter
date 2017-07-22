using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace AlphaECS.Unity
{
    public abstract class ComponentBehaviour : MonoBehaviour, IDisposable
    {
        protected IEventSystem EventSystem { get; set; }

        private CompositeDisposable _disposer = new CompositeDisposable();
        public CompositeDisposable Disposer
        {
            get { return _disposer; }
            private set { _disposer = value; }
        }

        protected bool IsQuitting = false;

        public virtual void OnDestroy()
        {
            Dispose();

            if (IsQuitting) return;
            EventSystem.Publish(new ComponentDestroyed() { Component = this });
        }

        [Inject]
		public virtual void Setup(IEventSystem eventSystem)
        {
			EventSystem = eventSystem;
            EventSystem.Publish(new ComponentCreated() { Component = this });
        }

        public virtual void Dispose()
        {
            Disposer.Dispose();
        }

        public virtual void OnApplicationQuit()
        {
            IsQuitting = true;
        }
    }
}
