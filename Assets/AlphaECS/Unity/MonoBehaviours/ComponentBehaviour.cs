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
		[Inject] public IEventSystem EventSystem { get; set; }
		[Inject] public IPoolManager PoolManager { get; set; }

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

		void OnDestroy()
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
