using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using AlphaECS.Unity;
using System.Linq;
using System;

namespace AlphaECS
{
	public static class EntityExtensions
	{
		public static string Serialize(this IEntity entity, params Type[] ignorableTypes)
		{
			var json = "{ \"Id\": " + "\"" + entity.Id + "\"" + ", \n";

			var types = "\"Types\": [";
			types += string.Join (",", entity.Components.Where(c => !c.ShouldIgnore(ignorableTypes)).Select (c => "\"" + c.GetType ().ToString () + "\" ").ToArray ()) + "], \n";
			json += types;

			json += "\"Components\": [";
			foreach (var component in entity.Components)
			{
				if (component.ShouldIgnore(ignorableTypes))
				{ continue; }
					
				var componentAsJson = JsonUtility.ToJson (component);
				json += componentAsJson + ", \n";
			}

			json = json.Remove (json.LastIndexOf(","), 1) + "]}";
			return json;
		}

		public static IEntity Deserialize(this IEntity entity, string json, params Type[] ignorableTypes)
		{
			var node = JSON.Parse (json);

			for (var i = 0; i < node["Types"].Count; i++)
			{
				var type = node ["Types"] [i].ToString().Replace("\"", "").GetTypeWithAssembly();

				if (type == null)
				{
					Debug.LogWarning ("Unable to resolve type " + node ["Types"] [i].ToString ().Replace ("\"", "") + "!");
					continue;
				}

				if (!entity.HasComponent (type))
				{
					Debug.LogWarning ("Type " + node ["Types"] [i].ToString ().Replace ("\"", "") + " not found on entity!");
					continue;
				}

				var component = entity.GetComponent (type);
				if (component == null)
				{
					if (component is Component)
					{
						if (!entity.HasComponent<ViewComponent> ())
						{
							Debug.LogWarning ("No view found for component " + type.ToString () + " !");
							continue;
						}

						var viewComponent = entity.GetComponent<ViewComponent> ();
						var transform = viewComponent.Transforms.FirstOrDefault ();

						if (transform == null)
						{
							Debug.LogWarning ("No transform found for component " + type.ToString () + " !");
							continue;
						}
						component = transform.gameObject.AddComponent (type);
					}
					else
					{
						component = (object)Activator.CreateInstance (type);
					}
				}

				if (ignorableTypes.Count() > 0)
				{
					if (component.ShouldIgnore(ignorableTypes))
					{ continue; }
				}

				JsonUtility.FromJsonOverwrite (node ["Components"] [i].ToString(), component);
			}
			return entity;
		}

		public static bool ShouldIgnore(this object component, Type[] ignorableTypes)
		{
			var shouldIgnore = false;

			if ((!(component is MonoBehaviour) && !(component is IComponent)) ||
				component.GetType ().IsDefined (typeof(NonSerializableDataAttribute), false))
			{
				shouldIgnore = true;
			}

			foreach (var type in ignorableTypes)
			{
                if (type.IsAssignableFrom(component.GetType()))
				{
					shouldIgnore = true;
					break;
				}
			}

			return shouldIgnore;
		}
	}
}