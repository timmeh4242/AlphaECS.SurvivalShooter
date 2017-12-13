using UnityEngine;
using Zenject;
using AlphaECS;
using System.Linq;
using System.Collections.Generic;
using AlphaECS.Unity;
using System.Collections;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		var systems = GetComponentsInChildren<SystemBehaviour> (true);
		foreach (var system in systems)
		{
			Container.Bind(system.GetType()).FromInstance (system).AsSingle ();
		}
    }

	public override void Start ()
	{
		base.Start ();

		var entityBehaviours = GetComponentsInChildren<EntityBehaviour> (true);

		foreach (var eb in entityBehaviours)
		{
			eb.gameObject.ForceEnable ();
			eb.gameObject.ForceEnable (); //HACK!!! for some reason we have to call this twice - not sure if it's a framework issue or project specific
		}
			
//		var gameObjects = entityBehaviours.Select (eb => eb.gameObject).ToArray();
//		gameObjects.ForceEnable ();
////		gameObjects.ForceEnable (); //HACK for some reason we have to call this twice - not sure if it's a framework bug or a project specific edge case...
	}
}