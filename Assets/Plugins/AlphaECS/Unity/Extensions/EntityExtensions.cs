using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using AlphaECS.Unity;
using System.Linq;
using System;
using UniRx;

namespace AlphaECS
{
	public static class EntityExtensions
	{
		public static string Serialize(this IEntity entity, Type[] includedTypes = null, Type[] ignoredTypes = null)
		{
			var json = "{ \"Id\": " + "\"" + entity.Id + "\"" + ", \n";

			var types = "\"Types\": [";
			types += string.Join (",", entity.Components.Where(c => 
				{
					if(includedTypes != null && includedTypes.Contains(c.GetType()))
					{
						return true;
					}
					else
					{
						if(!c.GetType().ShouldIgnore(ignoredTypes))
						{ return true; }
					}
					return false;
				}).Select (c => "\"" + c.GetType ().ToString () + "\" ").ToArray ()) + "], \n";
			json += types;

			json += "\"Components\": [";
			foreach (var component in entity.Components)
			{
				if (component.GetType().ShouldIgnore(ignoredTypes))
				{ continue; }
					
				var componentAsJson = JsonUtility.ToJson (component);
				json += componentAsJson + ", \n";
			}

			json = json.Remove (json.LastIndexOf(","), 1) + "]}";
			return json;
		}

		public static IEntity Deserialize(this IEntity entity, string json, Type[] includedTypes = null, Type[] ignoredTypes = null)
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

				if (includedTypes != null && !includedTypes.Contains (type))
				{ continue; }

				if (ignoredTypes != null && type.ShouldIgnore(ignoredTypes))
				{ continue; }

//				if (!entity.HasComponent (type))
//				{
//					Debug.LogWarning ("Type " + node ["Types"] [i].ToString ().Replace ("\"", "") + " not found on entity!");
//					continue;
//				}

				object component = null;

				if (typeof(Component).IsAssignableFrom(type))
				{					
					if (!entity.HasComponent<ViewComponent> ())
					{
						entity.AddComponent (new ViewComponent ());
						Debug.LogWarning ("No view found for component " + type.ToString () + " !");
					}

					var viewComponent = entity.GetComponent<ViewComponent> ();
					var index = i;
					viewComponent.Transforms.ObserveAdd ().Select(x => x.Value).StartWith(viewComponent.Transforms).Subscribe (t =>
					{
						if(t.gameObject.GetComponent(type) != null)
						{
							component = t.gameObject.GetComponent (type);
							JsonUtility.FromJsonOverwrite (node ["Components"] [index].ToString(), component);
						}
					}).AddTo (viewComponent.Disposer);
				}
				else
				{
					if (!entity.HasComponent (type))
					{
						component = (object)Activator.CreateInstance (type);
					}
					else
					{
						component = entity.GetComponent (type);
					}
					JsonUtility.FromJsonOverwrite (node ["Components"] [i].ToString(), component);
				}
			}
			return entity;
		}

		public static bool ShouldIgnore(this Type type, Type[] ignoredTypes)
		{
			var shouldIgnore = false;

			//use MonoBehaviour as JsonUtility.ToJson does not support engine types
			if ((!typeof(MonoBehaviour).IsAssignableFrom(type) && !typeof(IComponent).IsAssignableFrom(type)) ||
				type.IsDefined (typeof(NonSerializableDataAttribute), false))
			{
				shouldIgnore = true;
			}

			foreach (var t in ignoredTypes)
			{
                if (t.IsAssignableFrom(type))
				{
					shouldIgnore = true;
					break;
				}
			}

			return shouldIgnore;
		}
	}
}