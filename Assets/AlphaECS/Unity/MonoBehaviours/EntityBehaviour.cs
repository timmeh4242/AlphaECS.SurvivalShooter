using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using System;
using AlphaECS;
using AlphaECS.Json;

namespace AlphaECS.Unity
{
    public class EntityBehaviour : ComponentBehaviour
	{
		public IPool Pool
		{
			get
			{
				if(Proxy != null)
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
					return (pool = PoolManager.CreatePool (PoolName));
				}
				else
				{
					return (pool = PoolManager.GetPool (PoolName));
				}
			}
			set { pool = value; }
		}
		private IPool pool;

		public IEntity Entity
		{
			get
			{
				if(Proxy != null)
				{
					return Proxy.Entity;
				}
				return entity == null ? (entity = Pool.CreateEntity ()) : entity;
			}
			set
			{
				entity = value;
			}
		}
		private IEntity entity;

		[SerializeField]
		public EntityBehaviour Proxy;

		[SerializeField] [HideInInspector]
		public string PoolName;

		[SerializeField] [HideInInspector]
		public bool RemoveEntityOnDestroy = true;

		[SerializeField] [HideInInspector]
		public List<string> CachedComponents = new List<string>();

		[SerializeField] [HideInInspector]
		public List<string> CachedProperties = new List<string>();

		public override void Setup ()
		{
			base.Setup ();

			if(Entity.HasComponent<ViewComponent>() == false)
			{
				var viewComponent = new ViewComponent(){ DestroyWithView = true, View = gameObject };
				Entity.AddComponent(viewComponent);
			}

			for (var i = 0; i < CachedComponents.Count(); i++)
			{
				var typeName = CachedComponents[i];
				var type = Type.GetType(typeName);
				if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

				var component = (object)Activator.CreateInstance(type);
				var componentProperties = JSON.Parse(CachedProperties[i]);
				component.DeserializeComponent(componentProperties);

				Entity.AddComponent(component);
			}

			var monoBehaviours = GetComponents<Component> ();
			foreach (var mb in monoBehaviours)
			{
				if(mb.GetType() != typeof(EntityBehaviour) &&
					mb.GetType() != typeof(Transform))
				{
					Entity.AddComponent (mb);
				}
			}
		}

		public override void OnDestroy ()
		{
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

			base.OnDestroy ();
		} 
    }
}