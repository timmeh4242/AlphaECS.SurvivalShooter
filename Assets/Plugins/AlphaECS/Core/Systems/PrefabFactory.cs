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

        public GameObject Instantiate(GameObject prefab)
        {
            var gameObject = Container.InstantiatePrefab(prefab);
            return gameObject;
        }

        public GameObject Instantiate(GameObject prefab, Transform parent)
        {
            var gameObject = Container.InstantiatePrefab(prefab, parent);
            return gameObject;
        }

        public GameObject Instantiate(IEntity entity, GameObject prefab, Transform parent)
        {
            var gameObject = GameObject.Instantiate(prefab, parent);

            if (!gameObject.GetComponent<EntityBehaviour>())
            {
                throw new Exception("GameObject has no EntityBehaviour monobehaviour to link to!");
            }

            var entityBehaviour = gameObject.GetComponent<EntityBehaviour>();
            entityBehaviour.Entity = entity;
            entityBehaviour.RemoveEntityOnDestroy = false;

            Container.InjectGameObject(gameObject);

            return gameObject;
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
