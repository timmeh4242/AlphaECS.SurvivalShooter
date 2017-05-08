using System;
using UnityEngine;

namespace AlphaECS.Unity
{
	public static class GameObjectExtensions
	{
		public static void BindToEntity(this GameObject gameObject, IEntity entity, IPool pool)
		{
			if(gameObject.GetComponent<EntityBehaviour>())
			{ throw new Exception("GameObject already has an EntityBehaviour monobehaviour applied"); }

//            if(entity.HasComponents(typeof(EntityBehaviour))
//            { throw new Exception("Entity already has a EntityBehaviour monobehaviour applied"); }

			var entityBehaviour = gameObject.AddComponent<EntityBehaviour>();
			entityBehaviour.Entity = entity;
			entityBehaviour.Pool = pool;
		}
	}
}
