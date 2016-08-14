using System.Collections.Generic;
using EcsRx.Entities;
using EcsRx.Pools;
using UnityEngine;
using Zenject;
using System.Linq;
using EcsRx.Unity.Components;
using System;
using EcsRx.Components;
using EcsRx.Json;
using Assets.EcsRx.Unity.Extensions;

namespace EcsRx.Unity.MonoBehaviours
{
    public class EntityBehaviour : MonoBehaviour
    {
        public IPool Pool { get; set; }
        public IEntity Entity { get; set; }

		[Inject]
		public IPoolManager PoolManager { get; private set; }

		[SerializeField]
		public string PoolName;

		[SerializeField]
		public List<string> CachedComponents = new List<string>();

		[SerializeField]
		public List<string> CachedProperties = new List<string>();

		[Inject]
		public void RegisterEntity()
		{
			if (!gameObject.activeInHierarchy || !gameObject.activeSelf) { return; }

			IPool poolToUse;

			if (string.IsNullOrEmpty(PoolName))
			{ poolToUse = PoolManager.GetPool(); }
			else if (PoolManager.Pools.All(x => x.Name != PoolName))
			{ poolToUse = PoolManager.CreatePool(PoolName); }
			else
			{ poolToUse = PoolManager.GetPool(PoolName); }

			var createdEntity = poolToUse.CreateEntity();
			createdEntity.AddComponent(new ViewComponent { View = gameObject });
			SetupEntityBinding(createdEntity, poolToUse);
			SetupEntityComponents(createdEntity);

			Destroy(this);
		}

		private void SetupEntityBinding(IEntity entity, IPool pool)
		{
			var entityBinding = gameObject.AddComponent<EntityBehaviour>();
			entityBinding.Entity = entity;
			entityBinding.Pool = pool;
		}

		private void SetupEntityComponents(IEntity entity)
		{
			for (var i = 0; i < CachedComponents.Count(); i++)
			{
				var typeName = CachedComponents[i];
				var type = Type.GetType(typeName);
				if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

				var component = (IComponent)Activator.CreateInstance(type);
				var componentProperties = JSON.Parse(CachedProperties[i]);
				component.DeserializeComponent(componentProperties);

				entity.AddComponent(component);
			}
		}

		public IPool GetPool()
		{
			return PoolManager.GetPool(PoolName);
		}
    }
}