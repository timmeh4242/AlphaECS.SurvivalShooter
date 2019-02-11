using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public partial class CreateScriptAssetsMenu
{
    private static MethodInfo createScriptMethod = typeof(ProjectWindowUtil)
        .GetMethod("CreateScriptAsset", BindingFlags.Static | BindingFlags.NonPublic);

    static void CreateScriptAsset(string templatePath, string destName)
    {
        createScriptMethod.Invoke(null, new object[] { templatePath, destName });

        //string[] lines = File.ReadAllLines(destName);
        //foreach(var line in lines)
        //{
        //    line.Replace("SystemBehaviourTemplate", destName);
        //}
        //File.WriteAllLines(destName, lines);
    }
}

