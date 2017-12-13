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
				return entity == null ? (entity = Pool.CreateEntity(Id)) : entity;
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
		public string Id;

		[SerializeField] [HideInInspector]
		public string PoolName;

		[SerializeField] [HideInInspector]
		public bool RemoveEntityOnDestroy = true;

        //[SerializeField] [HideInInspector]
        //public List<ComponentBase> Components = new List<ComponentBase>();

        [SerializeField] [HideInInspector]
        public List<string> ComponentTypes = new List<string>();

		[SerializeField] [HideInInspector]
        public List<string> ComponentData = new List<string>();

		[SerializeField] [HideInInspector]
        public List<BlueprintBase> Blueprints = new List<BlueprintBase>();

		public IPoolManager PoolManager
		{
			get
			{
				return poolManager == null ? ProjectContext.Instance.Container.Resolve<IPoolManager>() : poolManager;
			}
			set { poolManager = value; }
		}
		private IPoolManager poolManager;

		public override void Initialize (IEventSystem eventSystem)
		{
			base.Initialize (eventSystem);
		}

		/* TODO
		 * this gets us around the "force enable" issue but
		 * we still may have a problem of garbage entities
		 * because the entity gets added to the pool with or without awake being called
		*/
		void Awake()
		{
			if (!Entity.HasComponent<ViewComponent> ())
			{
				var viewComponent = new ViewComponent();
				AddTransformToView(viewComponent);
				Entity.AddComponent (viewComponent);
			}
			else
			{
				AddTransformToView (Entity.GetComponent<ViewComponent> ());
			}

			for (var i = 0; i < ComponentTypes.Count(); i++)
			{
				var type = ComponentTypes[i].GetTypeWithAssembly();
				if (type == null) { throw new Exception("Cannot resolve type for [" + ComponentTypes[i] + "]"); }

				var component = (object)Activator.CreateInstance(type);
				JsonUtility.FromJsonOverwrite(ComponentData[i], component);
				Entity.AddComponent(component);
			}


			//for (var i = 0; i < Components.Count; i++)
			//{
			//             var component = Instantiate(Components[i]);
			//	Entity.AddComponent(component);
			//}

			foreach(var blueprint in Blueprints)
			{
				blueprint.Apply(this.Entity);
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
			if (EcsApplication.IsQuitting) { return; }
			if (!RemoveEntityOnDestroy) { return; }
			if (Proxy != null) { return; }

			this.Pool.RemoveEntity (Entity);

			base.OnDestroy();
		}

		private void AddTransformToView(ViewComponent viewComponent)
		{
			//ensure that the root EntityBehaviour's transform is first
			if (Proxy == null)
			{
				viewComponent.Transforms.Insert (0, this.transform);
			}
			else
			{
				viewComponent.Transforms.Add(this.transform);
			}
		}
	}
}