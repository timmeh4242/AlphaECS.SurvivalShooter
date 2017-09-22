//using System.Collections;
//using System.Collections.Generic;
//using AlphaECS.SurvivalShooter;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(MeleeEnemyBlueprint), true)]
//public class MeleeEnemyBlueprintEditor : Editor
//{
//    private MeleeEnemyBlueprint view;

//    private void OnEnable()
//    {
//		if (view == null)
//        { view = (MeleeEnemyBlueprint)target; }

//		var path = AssetDatabase.GetAssetPath(view);
//        if (string.IsNullOrEmpty(path))
//        { return;  }

//		if (view.Health == null)
//        {
//            view.Health = ScriptableObject.CreateInstance<HealthComponent>();
//            view.Health.name = "HealthComponent";
//            AssetDatabase.AddObjectToAsset(view.Health, view);
//        }

//        if (view.Melee == null)
//        {
//            view.Melee = ScriptableObject.CreateInstance<MeleeComponent>();
//			view.Melee.name = "MeleeComponent";
//			AssetDatabase.AddObjectToAsset(view.Melee, view);
//        }
//	}

//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//		//if (view == null)
//		//{ view = (MeleeEnemyBlueprint)target; }

//		//var path = AssetDatabase.GetAssetPath(view);
//		//if (string.IsNullOrEmpty(path))

//		//if (view.Health == null)
//		//{
//		//	view.Health = ScriptableObject.CreateInstance<HealthComponent>();
//		//	view.Health.name = "HealthComponent";
//		//	AssetDatabase.AddObjectToAsset(view.Health, view);
//		//}

//		//if (view.Melee == null)
//		//{
//		//	view.Melee = ScriptableObject.CreateInstance<MeleeComponent>();
//		//	view.Melee.name = "MeleeComponent";
//		//	AssetDatabase.AddObjectToAsset(view.Melee, view);
//		//}
//	}
//}
