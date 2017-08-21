using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

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
				return entity == null ? (entity = Pool.CreateEntity(CachedId)) : entity;
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

		[SerializeField] [HideInInspector]
		public string CachedId;

		[SerializeField] [HideInInspector]
		public string PoolName;

		[SerializeField] [HideInInspector]
		public bool RemoveEntityOnDestroy = true;

		[SerializeField] [HideInInspector]
		public List<string> CachedComponents = new List<string>();

		[SerializeField] [HideInInspector]
		public List<string> CachedProperties = new List<string>();

		public IPoolManager PoolManager
		{
			get
			{
				return poolManager == null ? ProjectContext.Instance.Container.Resolve<IPoolManager>() : poolManager;
			}
			set { poolManager = value; }
		}
		private IPoolManager poolManager;

		public override void Setup (IEventSystem eventSystem)
		{
			base.Setup (eventSystem);

			for (var i = 0; i < CachedComponents.Count(); i++)
			{
				var typeName = CachedComponents[i];
				var type = TypeUtilities.GetTypeWithAssembly(typeName);
				if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

				var component = (object)Activator.CreateInstance(type);
				var componentProperties = JSON.Parse(CachedProperties[i]);
				component.Deserialize(componentProperties);

				Entity.AddComponent(component);
			}

			if (!Entity.HasComponent<ViewComponent> ())
			{
				var viewComponent = new ViewComponent ();
				viewComponent.Transforms.Add (this.transform);
				Entity.AddComponent (viewComponent);
			}
			else
			{
				var viewComponent = Entity.GetComponent<ViewComponent> ();
				viewComponent.Transforms.Add (this.transform);
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
					if (mb.GetType() != typeof(Transform) && mb.GetType() != typeof(EntityBehaviour))
					{ Entity.AddComponent(mb); }
				}
			}
		}

		public override void OnDestroy()
		{
			if (IsQuitting) return;
			if (!RemoveEntityOnDestroy) return;
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
	}
}