using EcsRx.Systems;
using EcsRx.Unity.Components;
using UnityEngine;
using EcsRx.Unity.MonoBehaviours;
using EcsRx.Events;
using EcsRx.Groups;
using UniRx;
using Zenject;
using System.Collections;
using System;

namespace EcsRx.Systems
{
	public abstract class ReactiveSystemBehaviour : MonoBehaviour, IReactiveSystem
	{		
		[Inject] public IEventSystem EventSystem { get; set; }

		private CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			set { _disposer = value; }
		}

		[Inject]
		public virtual void Setup ()
		{
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

		void OnDestroy()
		{
			Dispose ();
		}
	}
}
