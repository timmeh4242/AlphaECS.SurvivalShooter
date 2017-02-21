using UnityEngine;
using Zenject;

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
    }
}