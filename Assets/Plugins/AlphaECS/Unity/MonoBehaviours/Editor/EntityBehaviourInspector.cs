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
    public class EntityBehaviourInspector : Editor
    {
        private EntityBehaviour view;

        private ReorderableList reorderableElements;

        private bool showComponents = true;

		private readonly IEnumerable<Type> allComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
							.SelectMany(s => s.GetTypes())
							.Where(p => typeof(ComponentBase).IsAssignableFrom(p) && p.IsClass);

		int lineHeight = 15;
		int lineSpacing = 18;

		private class ObjectInfo
		{
			public Type type;
		}

		void OnEnable()
		{
			hideFlags = HideFlags.HideAndDontSave;

			if (view == null)
            { view = (EntityBehaviour)target; }

			reorderableElements = new ReorderableList(serializedObject, serializedObject.FindProperty("CachedComponents"), true, true, true, true);

			reorderableElements.drawHeaderCallback = (Rect rect) =>
			{
				EditorGUI.LabelField(rect, "Components", EditorStyles.boldLabel);
			};

			reorderableElements.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = reorderableElements.serializedProperty.GetArrayElementAtIndex(index);
				var so = new SerializedObject(element.objectReferenceValue);
				so.Update();

                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), view.CachedComponents[index].GetType().ToString(), EditorStyles.boldLabel);

				var iterator = so.GetIterator();
				iterator.NextVisible(true); // skip the script reference

				var i = 1;
				var showChildren = true;
				while (iterator.NextVisible(showChildren))
				{
					EditorGUI.PropertyField(new Rect(rect.x, rect.y + (lineSpacing * i), rect.width, lineHeight), iterator);
					i++;
					if (iterator.isArray)
					{
						showChildren = iterator.isExpanded;
					}
				}

				so.ApplyModifiedProperties();
			};

			reorderableElements.elementHeightCallback = (int index) =>
			{
				float height = 0;

				var element = reorderableElements.serializedProperty.GetArrayElementAtIndex(index);
				var elementObj = new SerializedObject(element.objectReferenceValue);

				var iterator = elementObj.GetIterator();
				var i = 1;
				var showChildren = true;
				while (iterator.NextVisible(showChildren))
				{
					i++;
					if (iterator.isArray)
					{
						showChildren = iterator.isExpanded;
					}
				}

				height = lineSpacing * i;
				return height;
			};

			reorderableElements.onAddDropdownCallback = (Rect rect, ReorderableList list) =>
			{
				var dropdownMenu = new GenericMenu();
				var types = allComponentTypes.Select(x => x.Name).ToArray();

				for (var i = 0; i < types.Length; i++)
				{
					dropdownMenu.AddItem(new GUIContent(types[i]), false, AddAction, new ObjectInfo() { type = allComponentTypes.ElementAt(i) });
				}

				dropdownMenu.ShowAsContext();
			};

			reorderableElements.onRemoveCallback = (list) =>
			{
                var action = view.CachedComponents[list.index];
				DestroyImmediate(action, true);
				view.CachedComponents.RemoveAt(list.index);
			};
		}

		public override void OnInspectorGUI()
		{
			if (view == null)
            { view = (EntityBehaviour)target; }

			base.OnInspectorGUI();

			if (view == null) { return; }

			//EditorGUI.LabelField(rect, "Actions", EditorStyles.boldLabel);

			if (showComponents)
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

			if (showComponents)
			{
				serializedObject.Update();
				Undo.RecordObject(view, "Added Component");
				reorderableElements.DoLayoutList();
				PersistChanges();
				serializedObject.ApplyModifiedProperties();
			}
		}

		private void AddAction(object info)
		{
			var actionInfo = (ObjectInfo)info;
            var component = (ComponentBase)ScriptableObject.CreateInstance(actionInfo.type);
			//AssetDatabase.AddObjectToAsset(component, view);
            view.CachedComponents.Add(component);
		}

		private void PersistChanges()
		{
			if (GUI.changed && !Application.isPlaying)
			{
				this.SaveActiveSceneChanges();
			}
		}
	}
}