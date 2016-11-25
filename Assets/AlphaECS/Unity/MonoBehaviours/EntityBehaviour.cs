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
		public IPool Pool { get; set; }
		public IEntity Entity { get; set; }

		[SerializeField]
		public string PoolName;
		public bool RemoveEntityOnDestroy = true;

		[SerializeField]
		public List<string> CachedComponents = new List<string>();

		[SerializeField]
		public List<string> CachedProperties = new List<string>();
    }
}