using AlphaECS.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlphaECS.Unity
{
	public class EntityBehaviour : ComponentBehaviour
	{
		public IPool Pool
		{
			get
			{
				if (Proxy != null)
				{
					return Proxy.Pool;
				}
				else if (pool != null)
				{
					return pool;
				}
				else if (string.IsNullOrEmpty(PoolName))
				{
					return (pool = PoolManager.GetPool());
				}
				else if (PoolManager.Pools.All(x => x.Name != PoolName))
				{
					return (pool = PoolManager.CreatePool(PoolName));
				}
				else
				{
					return (pool = PoolManager.GetPool(PoolName));
				}
			}
			set { pool = value; }
		}
		private IPool pool;

		public IEntity Entity
		{
			get
			{
				if (Proxy != null)
				{
					return Proxy.Entity;
				}
				return entity == null ? (entity = Pool.CreateEntity()) : entity;
			}
			set
			{
				// TODO add some logic for killing rogue entities
				if (entity != null)
				{

				}
				entity = value;
			}
		}
		private IEntity entity;

		[SerializeField]
		public EntityBehaviour Proxy;

		[SerializeField]
		[HideInInspector]
		public string PoolName;

		[SerializeField]
		[HideInInspector]
		public bool RemoveEntityOnDestroy = true;

		[SerializeField]
		[HideInInspector]
		public List<string> CachedComponents = new List<string>();

		[SerializeField]
		[HideInInspector]
		public List<string> CachedProperties = new List<string>();

		public override void Setup()
		{
			base.Setup();

			for (var i = 0; i < CachedComponents.Count(); i++)
			{
				var typeName = CachedComponents[i];
				var type = GetTypeWithAssembly(typeName);
				if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

				var component = (object)Activator.CreateInstance(type);
				var componentProperties = JSON.Parse(CachedProperties[i]);
				component.DeserializeComponent(componentProperties);

				Entity.AddComponent(component);
			}

			var monoBehaviours = GetComponents<Component>();
			foreach (var mb in monoBehaviours)
			{
				if (mb == null)
				{
					Debug.LogWarning("Component on " + this.gameObject.name + " is null!");
				}
				else
				{
					if (mb.GetType() != typeof(Transform))
					{
						if (mb.GetType() == typeof(EntityBehaviour))
						{
							if (!Entity.HasComponent<EntityBehaviour>()) Entity.AddComponent(mb);
						}
						else
						{
							Entity.AddComponent(mb);
						}
					}
				}
			}
		}

		public override void OnDestroy()
		{
			if (IsQuitting) return;
			if (!RemoveEntityOnDestroy)return;
			if (Proxy != null) return;

			IPool poolToUse;

			if (string.IsNullOrEmpty(PoolName))
			{
				poolToUse = PoolManager.GetPool();
			}
			else if (PoolManager.Pools.All(x => x.Name != PoolName))
			{
				poolToUse = PoolManager.CreatePool(PoolName);
			}
			else
			{
				poolToUse = PoolManager.GetPool(PoolName);
			}

			poolToUse.RemoveEntity(Entity);

			base.OnDestroy();
		}

		private Type GetTypeWithAssembly(string typeName)
		{
			var type = Type.GetType(typeName);
			if (type != null) return type;
			foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = a.GetType(typeName);
				if (type != null)
					return type;
			}
			return null;
		}
	}
}