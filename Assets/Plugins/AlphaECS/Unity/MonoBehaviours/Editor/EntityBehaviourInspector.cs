using System;
using System.Collections.Generic;
using System.Linq;
using AlphaECS;
using SimpleJSON;
using AlphaECS.Unity;
using UniRx;
using UnityEditor;
using UnityEngine;

namespace AlphaECS
{
    [CustomEditor(typeof(EntityBehaviour))]
    public class EntityBehaviourInspector : Editor
    {
        private EntityBehaviour _view;

        public bool showComponents;

		// can't use object here, should enforce icomponent for when people want to add simple data types via inspector
//        private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
//                                .SelectMany(s => s.GetTypes())
//								.Where(p => typeof(object).IsAssignableFrom(p) && p.IsClass);

		private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
								.SelectMany(s => s.GetTypes())
								.Where(p => typeof(IComponent).IsAssignableFrom(p) && p.IsClass);


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI ();

			_view = (EntityBehaviour)target;

			if (Application.isPlaying && _view.Entity == null)
			{
				EditorGUILayout.LabelField("No Entity Assigned");
				return;
			}

			DrawPoolSection();
			EditorGUILayout.Space();
			DrawAddComponentSection();
			DrawComponentsSection();
			PersistChanges();
		}

        private void DrawPoolSection ()
        {
            this.UseVerticalBoxLayout(() =>
            {
				if(Application.isPlaying)
				{
					if (GUILayout.Button("Destroy Entity"))
					{
						_view.Pool.RemoveEntity(_view.Entity);
						Destroy(_view.gameObject);
					}

					this.UseVerticalBoxLayout(() =>
					{
						var id = _view.Entity.Id.ToString();
						this.WithLabelField("Entity Id: ", id);
					});

					this.UseVerticalBoxLayout(() =>
					{
						this.WithLabelField("Pool: ", _view.Pool.Name);
					});
				}
				else
				{
					this.UseVerticalBoxLayout(() =>
					{
//						var id = _view.Entity.Id.ToString();
//						this.WithLabelField("Entity Id: ", id);
						_view.CachedId = this.WithTextField("Id:", _view.CachedId);
					});

					_view.PoolName = this.WithTextField("Pool: ", _view.PoolName);
				}

				EditorGUILayout.BeginHorizontal();
				_view.RemoveEntityOnDestroy = EditorGUILayout.Toggle(_view.RemoveEntityOnDestroy);
				EditorGUILayout.LabelField("Remove Entity On Destroy");
				EditorGUILayout.EndHorizontal();
            });
        }

		private void DrawAddComponentSection()
		{
			this.UseVerticalBoxLayout(() =>
			{
				var availableTypes = allComponentTypes
					.Where(x => !_view.CachedComponents.Contains(x.ToString()))
					.ToArray();

				var types = availableTypes.Select(x => string.Format("{0} [{1}]", x.Name, x.Namespace)).ToArray();
				var index = -1;
				index = EditorGUILayout.Popup("Add Component", index, types);

				if(Application.isPlaying && index >= 0)
				{
					var component = (object)Activator.CreateInstance(availableTypes[index]);
					_view.Entity.AddComponent(component);
				}
				else if (index >= 0)
				{
					var component = (object)Activator.CreateInstance(availableTypes[index]);
					var componentName = component.ToString();
					_view.CachedComponents.Add(componentName);
					var json = component.Serialize();
					_view.CachedProperties.Add(json.ToString());
				}
			});
		}

        private void DrawComponentsSection()
        {
            EditorGUILayout.BeginVertical(EditorExtensions.DefaultBoxStyle);
			int numberOfComponents = !Application.isPlaying ? _view.CachedComponents.Count () : _view.Entity.Components.Count ();

            this.WithHorizontalLayout(() =>
            {
				this.WithLabel("Components (" + numberOfComponents + ")");
				if(showComponents)
				{
					if (this.WithIconButton("▾"))
					{
						showComponents = false;
					}
				}
				else
				{
					if (this.WithIconButton("▸"))						
					{
						showComponents = true;
					}
				}
            });

            var componentsToRemove = new List<int>();
            if (showComponents)
            {
				for (var i = 0; i < numberOfComponents; i++)
                {
                    this.UseVerticalBoxLayout(() =>
                    {
						Type componentType;
						if(Application.isPlaying)
						{
							componentType = _view.Entity.Components.ElementAt(i).GetType();
						}
						else
						{
							componentType = _view.CachedComponents[i].GetTypeWithAssembly();
						}

						var typeName = componentType == null ? "" : componentType.Name;
						var typeNamespace = componentType == null ? "" : componentType.Namespace;

                        this.WithVerticalLayout(() =>
                        {
                            this.WithHorizontalLayout(() =>
                            {
                                if (this.WithIconButton("-"))
                                {
                                    componentsToRemove.Add(i);
                                }

                                this.WithLabel(typeName);
                            });

                            EditorGUILayout.LabelField(typeNamespace);
                            EditorGUILayout.Space();
                        });

						if (componentType == null)
						{
							if (GUILayout.Button ("TYPE NOT FOUND. TRY TO CONVERT TO BEST MATCH?"))
							{
								componentType = _view.CachedComponents [i].TryGetConvertedType();
								if (componentType == null)
								{
									Debug.LogWarning ("UNABLE TO CONVERT " + _view.CachedComponents [i]);
									return;
								}
								else
								{
									Debug.LogWarning ("CONVERTED " + _view.CachedComponents [i] + " to " + componentType.ToString());
								}
							}
							else
							{
								return;
							}
						}

						if(componentType.IsSubclassOf(typeof(UnityEngine.Component)))
							return;

                        ShowComponentProperties(i, componentType);
                    });
                }
            }

            EditorGUILayout.EndVertical();

            for (var i = 0; i < componentsToRemove.Count(); i++)
            {
				if (Application.isPlaying)
				{
					var component = _view.Entity.Components.ElementAt(componentsToRemove [i]);
					_view.Entity.RemoveComponent(component);

					if(component.GetType().IsSubclassOf(typeof(UnityEngine.Component)))
						Destroy((UnityEngine.Component)component);
				} 
				else
				{
					_view.CachedComponents.RemoveAt (componentsToRemove [i]);
					_view.CachedProperties.RemoveAt (componentsToRemove [i]);
				}
            }
        }

		private void ShowComponentProperties(int index, Type type)
		{
			object component;

			if (Application.isPlaying)
			{
				component = _view.Entity.Components.ElementAt (index);
			}
			else
			{
				component = (object)Activator.CreateInstance(type);
				var node = JSON.Parse(_view.CachedProperties[index]);
				component.Deserialize(node);
			}

//			var members = component.GetType().GetMembers();
			foreach (var property in component.GetType().GetProperties())
			{
				bool isTypeSupported = true;

				EditorGUILayout.BeginHorizontal();
				var _type = property.PropertyType;
				var _value = property.GetValue(component, null);

				if (_type == typeof(int))
				{
					_value = EditorGUILayout.IntField(property.Name, (int)_value);
				}
				else if (_type == typeof (IntReactiveProperty))
				{
					var reactiveProperty = _value as IntReactiveProperty;
					reactiveProperty.Value = EditorGUILayout.IntField(property.Name, reactiveProperty.Value);
				}
				else if (_type == typeof(float))
				{
					_value = EditorGUILayout.FloatField(property.Name, (float)_value);
				}
				else if (_type == typeof(FloatReactiveProperty))
				{
					var reactiveProperty = _value as FloatReactiveProperty;
					reactiveProperty.Value = EditorGUILayout.FloatField(property.Name, reactiveProperty.Value);
				}
				else if (_type == typeof(bool))
				{
					_value = EditorGUILayout.Toggle(property.Name, (bool)_value);
				}
				else if (_type == typeof(BoolReactiveProperty))
				{
					var reactiveProperty = _value as BoolReactiveProperty;
					reactiveProperty.Value = EditorGUILayout.Toggle(property.Name, reactiveProperty.Value);
				}
				else if (_type == typeof(string))
				{
					_value = EditorGUILayout.TextField(property.Name, (string)_value);
				}
				else if (_type == typeof(StringReactiveProperty))
				{
					var reactiveProperty = _value as StringReactiveProperty;
					reactiveProperty.Value = EditorGUILayout.TextField(property.Name, reactiveProperty.Value);
				}
				else if (_type == typeof(Vector2))
				{
					_value = EditorGUILayout.Vector2Field(property.Name, (Vector2)_value);
				}
				else if (_type == typeof(Vector2ReactiveProperty))
				{
					var reactiveProperty = _value as Vector2ReactiveProperty;
					_value = EditorGUILayout.Vector2Field(property.Name, reactiveProperty.Value);
				}
				else if (_type == typeof(Vector3))
				{
					_value = EditorGUILayout.Vector3Field(property.Name, (Vector3)_value);
				}
				else if (_type == typeof(Vector3ReactiveProperty))
				{
					var reactiveProperty = _value as Vector3ReactiveProperty;
					_value = EditorGUILayout.Vector2Field(property.Name, reactiveProperty.Value);
				}
				else if (_type == typeof(Color))
				{
					_value = EditorGUILayout.ColorField(property.Name, (Color)_value);
				}
				else if (_type == typeof(ColorReactiveProperty))
				{
					var reactiveProperty = _value as ColorReactiveProperty;
					reactiveProperty.Value = EditorGUILayout.ColorField(property.Name, reactiveProperty.Value);
				}
//				else if (_type == typeof(Bounds))
//				{
//					_value = EditorGUILayout.BoundsField(property.Name, (Bounds)_value);
//				}
//				else if (_type == typeof(BoundsReactiveProperty))
//				{
//					var reactiveProperty = _value as BoundsReactiveProperty;
//					reactiveProperty.Value = EditorGUILayout.BoundsField(property.Name, reactiveProperty.Value);
//				}
//				else if (_type == typeof(Rect))
//				{
//					_value = EditorGUILayout.RectField(property.Name, (Rect)_value);
//				}
//				else if (_type == typeof(RectReactiveProperty))
//				{
//					var reactiveProperty = _value as RectReactiveProperty;
//					reactiveProperty.Value = EditorGUILayout.RectField(property.Name, reactiveProperty.Value);
//				}
				else if (_type == typeof(Enum))
				{
					_value = EditorGUILayout.EnumPopup(property.Name, (Enum)_value);
				}
				// else if (_type == typeof(Object))
				// {
				// 	_value = EditorGUILayout.ObjectField(property.Name, (Object)property.GetValue(), Object);
				// }
				else
				{
//					Debug.LogWarning("This property type is not supported: " + _type.Name + " - In component: " + component.GetType().Name);
					Debug.LogWarning("This property type is not supported!");
					isTypeSupported = false;
				}

				if (isTypeSupported == true)
				{
					property.SetValue(component, _value, null);
				}

				if (!Application.isPlaying)
				{
					_view.CachedComponents[index] = component.GetType().ToString();
					var json = component.Serialize();
					_view.CachedProperties[index] = json.ToString();
				}
				EditorGUILayout.EndHorizontal();
			}
		}

		private void PersistChanges()
		{
			if (GUI.changed && !Application.isPlaying)
			{ this.SaveActiveSceneChanges(); }
		}
    }
}