using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AlphaECS.Json;
using AlphaECS.Unity;
using System.Linq;
using System;

namespace AlphaECS
{
	public static class EntityExtensions
	{
        public static string Serialize(this IEntity entity)
		{
			var json = "{ \"Id\": " + entity.Id + ", \n";

			var types = "\"Types\": [";
			types += string.Join (",", entity.Components.Where(c => c is ComponentBehaviour || c is MonoBehaviour || c is IComponent).Select (c => "\"" + c.GetType ().ToString () + "\" ").ToArray ()) + "], \n";
			json += types;

			json += "\"Components\": [";
			foreach (var component in entity.Components)
			{
				if (component is ComponentBehaviour || component is MonoBehaviour || component is IComponent)
				{
					var j = JsonUtility.ToJson (component);
					json += j + ", \n";
				}
			}

			json = json.Remove (json.LastIndexOf(","), 1) + "]}";
			return json;
		}

        public static IEntity Deserialize(this IEntity entity, string json)
		{
			var node = JSON.Parse (json);

			for (var i = 0; i < node["Types"].Count; i++)
			{
				var type = node ["Types"] [i].ToString().Replace("\"", "").GetTypeWithAssembly();
				var component = entity.GetComponent (type);
				if (component == null)
				{ component = (object)Activator.CreateInstance (type); }

				JsonUtility.FromJsonOverwrite (node ["Components"] [i], component);
			}
			return entity;
		}
	}
}