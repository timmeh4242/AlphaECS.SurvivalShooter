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

		private readonly IEnumerable<Type> blueprintBaseTypes = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(s => s.GetTypes())
                    .Where(p => typeof(BlueprintBase).IsAssignableFrom(p) && p.IsClass);

        int headerProperties = 2;
		int lineHeight = 15;
		int lineSpacing = 18;

		private class ObjectInfo
		{
			public Type type;
		}

		void OnEnable()
		{
			if (view == null)
            { view = (EntityBehaviour)target; }

            if (PrefabUtility.GetPrefabType(view.gameObject) == PrefabType.None)
            {
                hideFlags = HideFlags.HideAndDontSave;
            }
            else
            {
                hideFlags = HideFlags.None;
            }

            reorderableComponents = new ReorderableList(serializedObject, serializedObject.FindProperty("CachedComponents"), true, true, true, true);
			
            reorderableComponents.drawHeaderCallback = (Rect rect) =>
			{ EditorGUI.LabelField(rect, "Components", EditorStyles.boldLabel); };

            reorderableComponents.drawElementCallback = (rect, index, isActive, isFocused) => 
            { OnDrawElement(reorderableComponents, view.CachedComponents[index].GetType().ToString(), rect, index, isActive, isFocused); };

            reorderableComponents.elementHeightCallback = (index) => 
            { return OnElementHeight(reorderableComponents, index); };

			reorderableComponents.onAddDropdownCallback = (Rect rect, ReorderableList list) =>
			{ OnAddDropdown(rect, list, AddComponent, componentBaseTypes.ToArray()); };

			reorderableComponents.onRemoveCallback = (list) =>
			{ RemoveComponent(list); };



			reorderableBlueprints = new ReorderableList(serializedObject, serializedObject.FindProperty("Blueprints"), true, true, true, true);

			reorderableBlueprints.drawHeaderCallback = (Rect rect) =>
			{ EditorGUI.LabelField(rect, "Blueprints", EditorStyles.boldLabel); };

			reorderableBlueprints.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var label = view.Blueprints[index] != null ? view.Blueprints[index].GetType().ToString() : "None";
                OnDrawElement(reorderableBlueprints, label, rect, index, isActive, isFocused);
            };

			reorderableBlueprints.elementHeightCallback = (index) =>
			{ return OnElementHeight(reorderableBlueprints, index); };

			reorderableBlueprints.onAddDropdownCallback = (Rect rect, ReorderableList list) =>
            { OnAddEmpty(rect, list, AddBlueprint, blueprintBaseTypes.ToArray()); };

			reorderableBlueprints.onRemoveCallback = (list) =>
			{ RemoveBlueprint(list); };
		}

        private void OnDrawElement(ReorderableList list, string labelName, Rect rect, int index, bool isActive, bool isFocused)
        {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);

			EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), labelName, EditorStyles.boldLabel);
            EditorGUI.ObjectField(new Rect(rect.x, rect.y + lineHeight, rect.width, lineHeight), element);

			if (element.objectReferenceValue == null)
			{ return; }

			var so = new SerializedObject(element.objectReferenceValue);
			so.Update();

			var iterator = so.GetIterator();
			iterator.NextVisible(true); // skip the script reference

            var i = headerProperties;
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
        }

        private float OnElementHeight(ReorderableList list, int index)
        {
			float height = 0;

			var element = list.serializedProperty.GetArrayElementAtIndex(index);

            if(element.objectReferenceValue == null)
            {
                return headerProperties * lineSpacing;
			}

            var so = new SerializedObject(element.objectReferenceValue);
			var iterator = so.GetIterator();
            var i = headerProperties;
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

		private void OnAddEmpty(Rect rect, ReorderableList list, Action<object> action, Type[] types)
		{
            //view.Blueprints.Add(null);
            var blueprints = list.serializedProperty.serializedObject.FindProperty("Blueprints");
            blueprints.arraySize += 1;
        }

		public override void OnInspectorGUI()
        {
            if (view == null)
            { view = (EntityBehaviour)target; }

            base.OnInspectorGUI();

            if (view == null) { return; }

            DrawHeaderSection();

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

                if(!Application.isPlaying)
                {
					reorderableComponents.DoLayoutList();
                }
                else
                {
					reorderableComponents.DoLayoutList();
                }
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
					//for (var i = 0; i < view.Entity.Components.Count(); i++)
					//{
					//                   var editor = Editor.CreateEditor((UnityEngine.Object)view.Entity.Components.ElementAt(i));
					//  editor.OnInspectorGUI();
					//}
				}
			}

            if(showComponents || showBlueprints)
            {
				PersistChanges();
				serializedObject.ApplyModifiedProperties();
            }
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
					view.CachedId = this.WithTextField("Id:", view.CachedId);
					view.PoolName = this.WithTextField("Pool: ", view.PoolName);
				}

				EditorGUILayout.BeginHorizontal();
				view.RemoveEntityOnDestroy = EditorGUILayout.Toggle(view.RemoveEntityOnDestroy);
				EditorGUILayout.LabelField("Remove Entity On Destroy");
				EditorGUILayout.EndHorizontal();

                if(Application.isPlaying)
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
			var actionInfo = (ObjectInfo)info;
            var component = (ComponentBase)ScriptableObject.CreateInstance(actionInfo.type);

   //         var prefabType = PrefabUtility.GetPrefabType(view.gameObject);
   //         if (prefabType == PrefabType.None)
   //         {
   //             Debug.Log("not a prefab");
   //             hideFlags = HideFlags.HideAndDontSave;
			//}
   //         else
   //         {
			//	Debug.Log("is a prefab");
			//	hideFlags = HideFlags.None;

   //             if (prefabType == PrefabType.Prefab)
   //             {
   //                 AssetDatabase.AddObjectToAsset(component, view.gameObject);
			//	}
   //             else
   //             {
			//		var prefab = PrefabUtility.GetPrefabParent(view.gameObject);
			//		AssetDatabase.AddObjectToAsset(component, prefab);
   //                 var mods = PrefabUtility.GetPropertyModifications(prefab);
   //                 foreach(var mod in mods)
   //                 {
   //                     Debug.Log(mod.value);
			//		}
			//	}

   //             PrefabUtility.ResetToPrefabState(view);
			//}
            view.CachedComponents.Add(component);
		}

		private void RemoveComponent(ReorderableList list)
		{
			var component = view.CachedComponents[list.index];
			hideFlags = HideFlags.HideAndDontSave;
			DestroyImmediate(component, true);
			view.CachedComponents.RemoveAt(list.index);

			//var prefabType = PrefabUtility.GetPrefabType(view.gameObject);
			//if (prefabType == PrefabType.None)
			//{
			//	hideFlags = HideFlags.HideAndDontSave;
			//}
			//else
			//{
			//	hideFlags = HideFlags.None;

			//	if (prefabType == PrefabType.Prefab)
			//	{
			//		//AssetDatabase.AddObjectToAsset(component, view.gameObject);
			//	}
			//	else
			//	{
			//		//var prefab = PrefabUtility.GetPrefabParent(view.gameObject);
			//		//AssetDatabase.AddObjectToAsset(component, prefab);
			//		//var mods = PrefabUtility.GetPropertyModifications(prefab);
			//		//foreach (var mod in mods)
			//		//{
			//		//  Debug.Log(mod.value);
			//		//}
			//	}

			//	//PrefabUtility.ResetToPrefabState(view);
			//}
		}

        private void AddBlueprint(object info)
        {
			var actionInfo = (ObjectInfo)info;
            var blueprint = (BlueprintBase)ScriptableObject.CreateInstance(actionInfo.type);
            view.Blueprints.Add(blueprint);
        }

        private void RemoveBlueprint(ReorderableList list)
        {
            //var blueprint = view.Blueprints[list.index];
            //DestroyImmediate(blueprint, true);
            view.Blueprints.RemoveAt(list.index);
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