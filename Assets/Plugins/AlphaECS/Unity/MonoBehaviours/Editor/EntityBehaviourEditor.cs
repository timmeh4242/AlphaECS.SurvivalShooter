using System;
using System.Collections.Generic;
using System.Linq;
using AlphaECS;
using SimpleJSON;
using AlphaECS.Unity;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;


namespace AlphaECS
{
	[CustomEditor(typeof(EntityBehaviour), true)]
	public class EntityBehaviourEditor : Editor
	{
		private EntityBehaviour view;

		private ReorderableList reorderableComponents;

		private ReorderableList reorderableBlueprints;

		private bool showComponents = true;
		private bool showBlueprints = true;

		private readonly IEnumerable<Type> componentBaseTypes = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => typeof(ComponentBase).IsAssignableFrom(p) && p.IsClass);

        //private readonly IEnumerable<Type> blueprintBaseTypes = AppDomain.CurrentDomain.GetAssemblies()
            //.SelectMany(s => s.GetTypes())
            //.Where(p => typeof(BlueprintBase).IsAssignableFrom(p) && p.IsClass);

		int headerProperties = 2;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float lineSpacing = EditorGUIUtility.standardVerticalSpacing;

		public float[] componentHeights = new float[100];
		public float[] blueprintHeights = new float[100];

        private int componentToRemove = -1;

		private class ObjectInfo
		{
			public Type type;
		}

		void OnEnable()
		{
			if (view == null)
			{ view = (EntityBehaviour)target; }

			reorderableComponents = new ReorderableList(serializedObject, serializedObject.FindProperty("Components"), true, true, true, true);

			reorderableComponents.drawHeaderCallback = (Rect rect) =>
			{ EditorGUI.LabelField(rect, "Components", EditorStyles.boldLabel); };

			reorderableComponents.drawElementCallback = (rect, index, isActive, isFocused) =>
			{
                OnDrawElement(rect, view.Components[index], index, componentHeights);
				//OnDrawElement(reorderableComponents, view.Components[index].GetType().ToString(), rect, index, isActive, isFocused, componentHeights);
			};

			reorderableComponents.elementHeightCallback = (index) =>
			{
				return componentHeights[index];
//              return OnElementHeight(reorderableComponents, index);
			};

			reorderableComponents.onAddDropdownCallback = (Rect rect, ReorderableList list) =>
			{
				OnAddDropdown(rect, list, AddComponent, componentBaseTypes.ToArray());
			};

			reorderableComponents.onRemoveCallback = (list) =>
			{
				RemoveComponent(list);
			};



			reorderableBlueprints = new ReorderableList(serializedObject, serializedObject.FindProperty("Blueprints"), true, true, true, true);

			reorderableBlueprints.drawHeaderCallback = (Rect rect) =>
			{ EditorGUI.LabelField(rect, "Blueprints", EditorStyles.boldLabel); };

			reorderableBlueprints.drawElementCallback = (rect, index, isActive, isFocused) =>
			{
				var label = view.Blueprints[index] != null ? view.Blueprints[index].GetType().ToString() : "None";
				OnDrawElement(reorderableBlueprints, label, rect, index, isActive, isFocused, blueprintHeights);
			};

			reorderableBlueprints.elementHeightCallback = (index) =>
			{
				return blueprintHeights[index];
//              return OnElementHeight(reorderableBlueprints, index);
			};

			reorderableBlueprints.onAddDropdownCallback = (Rect rect, ReorderableList list) =>
			{ OnAddDropdown(list); };

			reorderableBlueprints.onRemoveCallback = (list) =>
			{ RemoveBlueprint(list); };
		}

		public override void OnInspectorGUI()
		{
			if (view == null)
			{ view = (EntityBehaviour)target; }

			base.OnInspectorGUI();

			if (view == null) { return; }

			DrawHeaderSection();

            componentToRemove = -1;

			if (Application.isPlaying)
			{
				if (showComponents)
				{
					if (this.WithIconButton("▾"))
					{ showComponents = false; }
				}
				else
				{
					if (this.WithIconButton("▸"))
					{ showComponents = true; }
				}

				if (showComponents)
				{
					for (var i = 0; i < view.Entity.Components.Count(); i++)
					{
						var rect = EditorGUILayout.BeginVertical();
						OnDrawElement(rect, view.Entity.Components.ElementAt(i), i, componentHeights);
						EditorGUILayout.EndVertical();

						GUILayoutUtility.GetRect(0f, componentHeights[i]);
						EditorGUILayout.Space();
					}
				}

                if(componentToRemove > -1)
                {
                    var component = view.Entity.Components.ElementAt(componentToRemove);
                    view.Entity.RemoveComponent(component);

                    if (component.GetType().IsSubclassOf(typeof(Component)))
                    {
                        Destroy((UnityEngine.Object)component);
                    }
				}
				return;
			}

			if (showComponents)
			{
				if (this.WithIconButton("▾"))
				{ showComponents = false; }
			}
			else
			{
				if (this.WithIconButton("▸"))
				{ showComponents = true; }
			}

			if (showComponents)
			{
				serializedObject.Update();
				Undo.RecordObject(view, "Added Data");

				reorderableComponents.DoLayoutList();
				serializedObject.ApplyModifiedProperties();
			}

			if (componentToRemove > -1)
			{
                view.Components.RemoveAt(componentToRemove);

				//if (component.GetType().IsSubclassOf(typeof(Component)))
				//{
				//	Destroy((UnityEngine.Object)component);
				//}
			}

			if (showBlueprints)
			{
				if (this.WithIconButton("▾"))
				{ showBlueprints = false; }
			}
			else
			{
				if (this.WithIconButton("▸"))
				{ showBlueprints = true; }
			}

			if (showBlueprints)
			{
				serializedObject.Update();
				Undo.RecordObject(view, "Added Data");

				if (!Application.isPlaying)
				{
					reorderableBlueprints.DoLayoutList();
				}
				else
				{
					reorderableBlueprints.DoLayoutList();
				}
				serializedObject.ApplyModifiedProperties();
			}

			if (showComponents || showBlueprints)
			{
				PersistChanges();
			}
		}

        /// <summary>
        /// Draws an element of type serialized property in a reorderable list
        /// </summary>
		private void OnDrawElement(ReorderableList list, string labelName, Rect rect, int index, bool isActive, bool isFocused, float[] heightsArray)
		{
			var element = list.serializedProperty.GetArrayElementAtIndex(index);

			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), labelName, EditorStyles.boldLabel);
			EditorGUI.ObjectField(new Rect(rect.x, rect.y + lineHeight, rect.width, lineHeight), element);

			if (element.objectReferenceValue == null)
			{
                heightsArray[index] = (headerProperties * lineHeight) + (headerProperties * lineSpacing);
				return;
			}

			var so = new SerializedObject(element.objectReferenceValue);
			so.Update();

			var iterator = so.GetIterator();
			iterator.NextVisible(true); // skip the script reference

			var i = headerProperties;
			var showChildren = true;
			while (iterator.NextVisible(showChildren))
			{
                EditorGUI.PropertyField(new Rect(rect.x, rect.y + (i * lineHeight) + (i * lineSpacing), rect.width, lineHeight), iterator);
				i++;
				if (iterator.isArray)
				{
					showChildren = iterator.isExpanded;
				}
			}

			so.ApplyModifiedProperties();
            heightsArray[index] = (i * lineHeight) + (i * lineSpacing);
		}

		/// <summary>
		/// Draws an element of type object in a list
		/// </summary>
		private void OnDrawElement(Rect rect, object component, int index, float[] heightsArray)
		{
			var componentType = component.GetType();
			var typeName = componentType == null ? "" : componentType.Name;
			var typeNamespace = componentType == null ? "" : componentType.Namespace;
			headerProperties = 0;

			if (string.IsNullOrEmpty(typeName))
			{
				typeName = "Unknown Type";
			}
			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), typeName, EditorStyles.boldLabel);
			headerProperties += 1;

			//EditorGUILayout.BeginHorizontal();
			if (GUI.Button(new Rect(rect.width, rect.y, lineHeight, lineHeight), "-"))
			{
				componentToRemove = index;
			}
			//EditorGUILayout.EndHorizontal();

			if (!string.IsNullOrEmpty(typeNamespace))
			{
				EditorGUI.LabelField(new Rect(rect.x, rect.y + lineHeight, rect.width, lineHeight), typeNamespace);
				headerProperties += 1;
			}

			//EditorGUILayout.Space();

			if (componentType.IsSubclassOf(typeof(UnityEngine.Component)))
			{
				heightsArray[index] = (headerProperties * lineHeight) + (headerProperties * lineSpacing);
				return;
			}

			var i = headerProperties;

			//draw component fields
			foreach (var field in component.GetType().GetFields())
			{

				var _type = field.FieldType;
				var _value = field.GetValue(component);
				var isTypeSupported = TryDrawValue(rect, _type, ref _value, field.Name, i);

				if (isTypeSupported == true)
				{
					field.SetValue(component, _value);
					i++;
				}
			}

			//draw component properties
			foreach (var property in component.GetType().GetProperties())
			{
				var _type = property.PropertyType;
				var _value = property.GetValue(component, null);
				var isTypeSupported = TryDrawValue(rect, _type, ref _value, property.Name, i);

				if (isTypeSupported == true)
				{
					property.SetValue(component, _value, null);
					i++;
				}
			}

			heightsArray[index] = (i * lineHeight) + (i * lineSpacing);
		}

		private bool TryDrawValue(Rect rect, Type type, ref object value, string name, int index)
		{
			if (name == "hideFlags" || name == "name")
			{
				return false;
			}

			if (type == typeof(int))
			{
				value = EditorGUI.IntField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (int)value);
			}
			else if (type == typeof(IntReactiveProperty))
			{
				var reactiveProperty = value as IntReactiveProperty;
				reactiveProperty.Value = EditorGUI.IntField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(float))
			{
				value = EditorGUI.FloatField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (float)value);
			}
			else if (type == typeof(FloatReactiveProperty))
			{
				var reactiveProperty = value as FloatReactiveProperty;
				reactiveProperty.Value = EditorGUI.FloatField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(bool))
			{
				value = EditorGUI.Toggle(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (bool)value);
			}
			else if (type == typeof(BoolReactiveProperty))
			{
				var reactiveProperty = value as BoolReactiveProperty;
				reactiveProperty.Value = EditorGUI.Toggle(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(string))
			{
				value = EditorGUI.TextField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (string)value);
			}
			else if (type == typeof(StringReactiveProperty))
			{
				var reactiveProperty = value as StringReactiveProperty;
				reactiveProperty.Value = EditorGUI.TextField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(Vector2))
			{
				value = EditorGUI.Vector2Field(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Vector2)value);
			}
			else if (type == typeof(Vector2ReactiveProperty))
			{
				var reactiveProperty = value as Vector2ReactiveProperty;
				value = EditorGUI.Vector2Field(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(Vector3))
			{
				value = EditorGUI.Vector3Field(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Vector3)value);
			}
			else if (type == typeof(Vector3ReactiveProperty))
			{
				var reactiveProperty = value as Vector3ReactiveProperty;
				value = EditorGUI.Vector3Field(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(Color))
			{
				value = EditorGUI.ColorField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Color)value);
			}
			else if (type == typeof(ColorReactiveProperty))
			{
				var reactiveProperty = value as ColorReactiveProperty;
				reactiveProperty.Value = EditorGUI.ColorField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, reactiveProperty.Value);
			}
			else if (type == typeof(Bounds))
			{
				value = EditorGUI.BoundsField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Bounds)value);
			}
			else if (type == typeof(BoundsReactiveProperty))
			{
				var reactiveProperty = value as BoundsReactiveProperty;
				reactiveProperty.Value = EditorGUI.BoundsField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Bounds)reactiveProperty.Value);
			}
			else if (type == typeof(Rect))
			{
				value = EditorGUI.RectField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Rect)value);
			}
			else if (type == typeof(RectReactiveProperty))
			{
				var reactiveProperty = value as RectReactiveProperty;
				reactiveProperty.Value = EditorGUI.RectField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Rect)reactiveProperty.Value);
			}
			else if (type == typeof(Enum))
			{
				value = EditorGUI.EnumPopup(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (Enum)value);
			}
			else if (type == typeof(UnityEngine.GameObject))
			{
				value = EditorGUI.ObjectField(new Rect(rect.x, rect.y + (index * lineHeight) + (index * lineSpacing), rect.width, lineHeight), name, (UnityEngine.GameObject)value, type, true);
			}
			else
			{
				Debug.LogWarning("This type is not supported: " + type.Name + " - In action: " + name);
				//Debug.LogWarning("This type is not supported!");
				return false;
			}

			return true;
		}

		private void OnAddDropdown(Rect rect, ReorderableList list, Action<object> action, Type[] types)
		{
			var dropdownMenu = new GenericMenu();

			for (var i = 0; i < types.Length; i++)
			{
				dropdownMenu.AddItem(new GUIContent(types[i].ToString()), false, action.Invoke, new ObjectInfo() { type = types.ElementAt(i) });
			}

			dropdownMenu.ShowAsContext();
		}

		private void OnAddDropdown(ReorderableList list)
		{
			//view.Blueprints.Add(null);
			var blueprints = list.serializedProperty.serializedObject.FindProperty("Blueprints");
			blueprints.arraySize += 1;
		}

		private void DrawHeaderSection()
		{
			this.UseVerticalBoxLayout(() =>
			{
				if (Application.isPlaying)
				{
					this.WithLabelField("Id: ", view.Entity.Id.ToString());
					this.WithLabelField("Pool: ", view.Pool.Name);
				}
				else
				{
					view.Id = this.WithTextField("Id:", view.Id);
					view.PoolName = this.WithTextField("Pool: ", view.PoolName);
				}

				EditorGUILayout.BeginHorizontal();
				view.RemoveEntityOnDestroy = EditorGUILayout.Toggle(view.RemoveEntityOnDestroy);
				EditorGUILayout.LabelField("Remove Entity On Destroy");
				EditorGUILayout.EndHorizontal();

				if (Application.isPlaying)
				{
					if (GUILayout.Button("Destroy Entity"))
					{
						view.Pool.RemoveEntity(view.Entity);
						Destroy(view.gameObject);
					}
				}
			});
		}

		private void AddComponent(object info)
		{
			var componentInfo = (ObjectInfo)info;
            var component = (ComponentBase)Activator.CreateInstance(componentInfo.type);
			//component.name = componentInfo.type.Name;
			view.Components.Add(component);
		}

		private void RemoveComponent(ReorderableList list)
		{
			var component = view.Components[list.index];
			view.Components.RemoveAt(list.index);
		}

		private void AddBlueprint(object info)
		{
			var blueprintInfo = (ObjectInfo)info;
			var blueprint = (BlueprintBase)ScriptableObject.CreateInstance(blueprintInfo.type);
			blueprint.name = blueprintInfo.type.Name;
			view.Blueprints.Add(blueprint);
		}

		private void RemoveBlueprint(ReorderableList list)
		{
			view.Blueprints.RemoveAt(list.index);
		}

		private void PersistChanges()
		{
			if (GUI.changed && !Application.isPlaying)
			{
				this.SaveActiveSceneChanges();
				//              AssetDatabase.SaveAssets();
			}
		}

		private void SetHideFlags(PrefabType prefabType)
		{
			if (prefabType == PrefabType.None)
			{
				hideFlags = HideFlags.HideAndDontSave;
			}
			else
			{
				hideFlags = HideFlags.None;
			}
		}
	}
}