using UnityEditor;
using UnityEngine;

public partial class CreateScriptAssetsMenu
{
    [MenuItem("Assets/Create/SystemBehaviour")]
    public static void CreateSystemBehaviour()
    {
        CreateScriptAssetsMenu.CreateScriptAsset("Assets/Plugins/AlphaECS/Unity/ScriptTemplates/SystemBehaviourTemplate.txt", "NewSystemBehaviour.cs");
    }

    [MenuItem("Assets/Create/ComponentBehaviour")]
    public static void CreateComponentBehaviour()
    {
        CreateScriptAssetsMenu.CreateScriptAsset("Assets/Plugins/AlphaECS/Unity/ScriptTemplates/ComponentBehaviourTemplate.txt", "NewComponentBehaviour.cs");
    }
}
