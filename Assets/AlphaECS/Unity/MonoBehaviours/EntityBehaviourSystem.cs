using UnityEngine;
using System.Collections;
using AlphaECS.Unity;
using AlphaECS;
using System;
using UniRx;
using System.Linq;
using Zenject;
using AlphaECS.Json;

namespace AlphaECS.Unity
{
	public class EntityBehaviourSystem : SystemBehaviour
	{
		[Inject] protected DiContainer Container = null;

		public override void Setup ()
		{
			base.Setup ();

			EventSystem.OnEvent<ComponentCreated>()
				.Where(x => x.Component is EntityBehaviour)
				.Select(x => x.Component as EntityBehaviour)
				.Where(x => x.Entity == null)
				.Subscribe(eb =>
			{
				if (!eb.gameObject.activeInHierarchy || !eb.gameObject.activeSelf)
					return;

				IPool poolToUse;

				if (string.IsNullOrEmpty(eb.PoolName))
				{
					poolToUse = PoolManager.GetPool();
				}
				else if (PoolManager.Pools.All(x => x.Name != eb.PoolName))
				{
					poolToUse = PoolManager.CreatePool(eb.PoolName);
				}
				else
				{
					poolToUse = PoolManager.GetPool(eb.PoolName);
				}

				var createdEntity = poolToUse.CreateEntity();
				eb.Entity = createdEntity;
				eb.Pool = poolToUse;
				if(eb.Entity.HasComponent<ViewComponent>() == false)
				{
					var viewComponent = new ViewComponent(){ DestroyWithView = true, View = eb.gameObject };
					eb.Entity.AddComponent(viewComponent);
				}

				for (var i = 0; i < eb.CachedComponents.Count(); i++)
				{
					var typeName = eb.CachedComponents[i];
					var type = Type.GetType(typeName);
					if (type == null) { throw new Exception("Cannot resolve type for [" + typeName + "]"); }

					var component = (object)Activator.CreateInstance(type);
					var componentProperties = JSON.Parse(eb.CachedProperties[i]);
					component.DeserializeComponent(componentProperties);

					eb.Entity.AddComponent(component);
				}

				var monoBehaviours = eb.GetComponents<Component> ();
				foreach (var mb in monoBehaviours)
				{
					if(mb.GetType() != typeof(EntityBehaviour) &&
						mb.GetType() != typeof(Transform))
					{
						eb.Entity.AddComponent (mb);
					}
				}
			}).AddTo(Disposer);

			EventSystem.OnEvent<ComponentDestroyed> ()
				.Where (x => x.Component is EntityBehaviour)
				.Select (x => x.Component as EntityBehaviour)
				.Where(x => x.RemoveEntityOnDestroy == true)
				.Subscribe (eb =>
			{
					IPool poolToUse;

					if (string.IsNullOrEmpty(eb.PoolName))
					{
						poolToUse = PoolManager.GetPool();
					}
					else if (PoolManager.Pools.All(x => x.Name != eb.PoolName))
					{
						poolToUse = PoolManager.CreatePool(eb.PoolName);
					}
					else
					{
						poolToUse = PoolManager.GetPool(eb.PoolName);
					}

					poolToUse.RemoveEntity(eb.Entity);
			}).AddTo (this);
		}
	}
}
