using System.Collections;
using System.Collections.Generic;
using AlphaECS.SurvivalShooter;
using UnityEngine;

[CreateAssetMenu(menuName = "Blueprint/Player")]
public class PlayerBlueprint : BlueprintBase
{
    public TestClass TestClass1 = new TestClass();
	public TestClass TestClass2 = new TestClass();
    public GameObject Target;
}
