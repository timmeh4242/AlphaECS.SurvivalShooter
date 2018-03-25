using UnityEngine;
using Zenject;
using AlphaECS.Unity;
using System;

namespace AlphaECS
{
    public class PrefabFactory
    {
        [Inject]
        protected DiContainer Container = null;
		[Inject]
		protected IEventSystem EventSystem = null;

//		public GameObject Instantiate(GameObject prefab)
//		{
//			return Instantiate(prefab, null);
//		}
//
//        public GameObject Instantiate(GameObject prefab, Transform parent)
//        {
//			return Instantiate(prefab, parent, true);
//        }
//
//		public GameObject Instantiate(GameObject prefab, bool fastInject)
//		{
//			return Instantiate(prefab, fastInject, null);
//		}

		public GameObject Instantiate(GameObject prefab, Transform parent = null, bool fastInject = true)
		{
			GameObject gameObject = null;
			if (fastInject)
			{
				var wasActive = prefab.activeSelf;
				if (wasActive)
				{
					#if UNITY_EDITOR
					Container.DefaultParent.gameObject.SetActive(false);
					prefab = GameObject.Instantiate (prefab, Container.DefaultParent);
					#endif

					prefab.SetActive (false);
				}
				gameObject = parent == null ? GameObject.Instantiate (prefab) : GameObject.Instantiate (prefab, parent);

				gameObject.name = prefab.name;

				Inject (gameObject);

				if (wasActive)
				{
					#if UNITY_EDITOR
					GameObject.Destroy(prefab);
					Container.DefaultParent.gameObject.SetActive(true);
					#else
					prefab.SetActive(true);
					#endif
					gameObject.SetActive (true);
				}
			}
			else
			{
				gameObject = parent == null ? Container.InstantiatePrefab (prefab) : Container.InstantiatePrefab (prefab, parent);
				gameObject.name = prefab.name;
			}

			if (!gameObject.activeInHierarchy)
			{ gameObject.ForceEnable (); }

			return gameObject;
		}

        public GameObject Instantiate(IEntity entity, GameObject prefab, Transform parent)
        {
			return Instantiate(entity, prefab, parent, true);
        }

		public GameObject Instantiate(IEntity entity, GameObject prefab, Transform parent, bool fastInject)
		{
			var gameObject = GameObject.Instantiate(prefab, parent);
			gameObject.name = prefab.name;

			if (!gameObject.GetComponent<EntityBehaviour>())
			{
				throw new Exception("GameObject has no EntityBehaviour monobehaviour to link to!");
			}

			var entityBehaviour = gameObject.GetComponent<EntityBehaviour>();
			entityBehaviour.Entity = entity;
			entityBehaviour.RemoveEntityOnDestroy = false;

			if (!gameObject.activeInHierarchy)
			{ gameObject.ForceEnable (); }

			if (fastInject)
			{
				Inject (gameObject);
			}
			else
			{
				Container.InjectGameObject(gameObject);
			}

			return gameObject;
		}

		private void Inject(GameObject gameObject)
		{
			var components = gameObject.GetComponentsInChildren<ComponentBehaviour> (true);
			foreach (var component in components)
			{ component.Initialize (EventSystem); }
		}

        public Transform DefaultParent
        {
            get
            {
                return Container.DefaultParent;
            }
        }
    }
}
