using UnityEngine;
using Zenject;
using AlphaECS;
using System.Linq;
using System.Collections.Generic;
using AlphaECS.Unity;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		var systems = GetComponentsInChildren<SystemBehaviour> ();
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
		}
	}
}