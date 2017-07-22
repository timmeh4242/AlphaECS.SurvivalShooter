using System.Reflection;
using AlphaECS.Json;
using UnityEngine;
using UniRx;

namespace AlphaECS
{ 
	public static class ComponentExtensions
	{
		public static JSONClass Serialize(this object component)
		{
			var node = new JSONClass();
			foreach (var property in component.GetType().GetProperties())
			{
				if (property.CanRead && property.CanWrite)
				{
                    if (property.PropertyType == typeof(int) || property.PropertyType.IsEnum)
					{
						node.Add(property.Name, new JSONData((int)property.GetValue(component, null)));
						continue;
					}
					if (property.PropertyType == typeof(IntReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as IntReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new IntReactiveProperty ();						
						node.Add(property.Name, new JSONData((int)reactiveProperty.Value));
						continue;
					}
					if (property.PropertyType == typeof(float))
					{
						node.Add(property.Name, new JSONData((float)property.GetValue(component, null)));
						continue;
					}
					if (property.PropertyType == typeof(FloatReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as FloatReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new FloatReactiveProperty ();						
						node.Add(property.Name, new JSONData((float)reactiveProperty.Value));
						continue;
					}
					if (property.PropertyType == typeof(bool))
					{
						node.Add(property.Name, new JSONData((bool)property.GetValue(component, null)));
						continue;
					}
					if (property.PropertyType == typeof(BoolReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as BoolReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new BoolReactiveProperty ();						
						node.Add(property.Name, new JSONData((bool)reactiveProperty.Value));
						continue;
					}
					if (property.PropertyType == typeof(string))
					{
						node.Add(property.Name, new JSONData((string)property.GetValue(component, null)));
						continue;
					}
					if (property.PropertyType == typeof(StringReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as StringReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new StringReactiveProperty ();						
						node.Add(property.Name, new JSONData((string)reactiveProperty.Value));
						continue;
					}
					if (property.PropertyType == typeof(Vector2))
					{
						node.Add(property.Name, new JSONData((Vector2)property.GetValue(component, null)));
						continue;
					}
					if (property.PropertyType == typeof(Vector2ReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as Vector2ReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new Vector2ReactiveProperty ();					
						node.Add(property.Name, new JSONData((Vector2)reactiveProperty.Value));
						continue;
					}
					if (property.PropertyType == typeof(Vector3))
					{
						node.Add(property.Name, new JSONData((Vector3)property.GetValue(component, null)));
						continue;
					}
					if (property.PropertyType == typeof(Vector3ReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as Vector3ReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new Vector3ReactiveProperty ();						
						node.Add(property.Name, new JSONData((Vector3)reactiveProperty.Value));
						continue;
					}

					if (property.PropertyType == typeof(Color))
					{
						var value = (Color)property.GetValue(component, null);
						var data = new JSONClass ();
						data.Add("r", new JSONData(value.r));
						data.Add("g", new JSONData(value.g));
						data.Add("b", new JSONData(value.b));
						data.Add("a", new JSONData(value.a));

						node.Add(property.Name, data);
						continue;
					}

					if (property.PropertyType == typeof(ColorReactiveProperty))
					{
						var reactiveProperty = property.GetValue(component, null) as ColorReactiveProperty;
						if (reactiveProperty == null)
							reactiveProperty = new ColorReactiveProperty ();	

						var data = new JSONClass ();
						data.Add("r", new JSONData(reactiveProperty.Value.r));
						data.Add("g", new JSONData(reactiveProperty.Value.g));
						data.Add("b", new JSONData(reactiveProperty.Value.b));
						data.Add("a", new JSONData(reactiveProperty.Value.a));

						node.Add(property.Name, data);
						continue;
					}
				}
			}
			return node;
		}

		public static void Deserialize(this object component, JSONNode node)
		{
			foreach (var property in component.GetType().GetProperties())
			{
				ApplyValue(component, node, property);
			}
		}

		private static void ApplyValue(object component, JSONNode node, PropertyInfo property)
		{
			if (property.CanRead && property.CanWrite)
			{
				var propertyData = node[property.Name];
				if (propertyData == null) return;

				if (property.PropertyType == typeof(int) || property.PropertyType.IsEnum)
				{
					property.SetValue(component, propertyData.AsInt, null);
					return;
				}
				if (property.PropertyType == typeof(IntReactiveProperty))
				{
					var reactiveProperty = new IntReactiveProperty(propertyData.AsInt);
					property.SetValue(component, reactiveProperty, null);
					return;
				}
				if (property.PropertyType == typeof(float))
				{
					property.SetValue (component, propertyData.AsFloat, null);
					return;
				}
				if (property.PropertyType == typeof(FloatReactiveProperty))
				{
					var reactiveProperty = new FloatReactiveProperty(propertyData.AsFloat);
					property.SetValue(component, reactiveProperty, null);
					return;
				}
				if (property.PropertyType == typeof(bool))
				{
					property.SetValue(component, propertyData.AsBool, null);
					return;
				}
				if (property.PropertyType == typeof(BoolReactiveProperty))
				{
					var reactiveProperty = new BoolReactiveProperty(propertyData.AsBool);
					property.SetValue(component, reactiveProperty, null);
					return;
				}
				if (property.PropertyType == typeof(string))
				{
					property.SetValue(component, propertyData.Value, null);
					return;
				}
				if (property.PropertyType == typeof(StringReactiveProperty))
				{
					var reactiveProperty = new StringReactiveProperty(propertyData.Value);
					property.SetValue(component, reactiveProperty, null);
					return;
				}
				if (property.PropertyType == typeof(Vector2))
				{
					property.SetValue(component, propertyData.AsVector2, null);
					return;
				}
				if (property.PropertyType == typeof(Vector2ReactiveProperty))
				{
					var reactiveProperty = new Vector2ReactiveProperty(propertyData.AsVector2);
					property.SetValue(component, reactiveProperty, null);
					return;
				}
				if (property.PropertyType == typeof(Vector3))
				{
					property.SetValue(component, propertyData.AsVector3, null);
					return;
				}
				if (property.PropertyType == typeof(Vector3ReactiveProperty))
				{
					var reactiveProperty = new Vector3ReactiveProperty(propertyData.AsVector3);
					property.SetValue(component, reactiveProperty, null);
					return;
				}
			}
		}
	}
}
