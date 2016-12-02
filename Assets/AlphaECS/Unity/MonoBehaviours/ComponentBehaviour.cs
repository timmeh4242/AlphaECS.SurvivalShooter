using UnityEngine;
using AlphaECS;
using UniRx;
using Zenject;
using System.Collections;
using System;

namespace AlphaECS.Unity
{
	public abstract class ComponentBehaviour : MonoBehaviour, IDisposable
	{
		public IEventSystem EventSystem
		{
			get
			{
				return eventSystem == null ? ProjectContext.Instance.Container.Resolve<IEventSystem> () : eventSystem;
			}
			set { eventSystem = value; }
		}
		private IEventSystem eventSystem;

		public IPoolManager PoolManager
		{
			get
			{
				return poolManager == null ? ProjectContext.Instance.Container.Resolve<IPoolManager> () : poolManager;
			}
			set { poolManager = value; }
		}
		private IPoolManager poolManager;

		private CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			set { _disposer = value; }
		}

//		void Awake()
//		{
//			EventSystem = ProjectContext.Instance.Container.Resolve<IEventSystem> ();
//			EventSystem.Publish (new ComponentCreated (){ Component = this });
//		}

		public virtual void OnDestroy()
		{
			Dispose ();
			if(EventSystem == null)
			{
				Debug.LogWarning ("A COMPONENT ON " + this.gameObject.name + " WAS NOT INJECTED PROPERLY!");
				EventSystem = ProjectContext.Instance.Container.Resolve<IEventSystem> ();
			}
			EventSystem.Publish (new ComponentDestroyed (){ Component = this });
		}

		[Inject]
		public virtual void Setup ()
		{
			EventSystem.Publish (new ComponentCreated (){ Component = this });
		}

		[Inject]
		public virtual IEnumerator SetupAsync ()
		{
			yield break;
		}

		public virtual void Dispose ()
		{
			Disposer.Dispose ();
		}
	}
}
