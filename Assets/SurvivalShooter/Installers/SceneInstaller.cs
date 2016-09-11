using UnityEngine;
using Zenject;
using EcsRx;
using System.Linq;
using System.Collections.Generic;
using EcsRx.SurvivalShooter;
using EcsRx.Unity;

public class SceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		var systems = GetComponentsInChildren<SystemBehaviour> ();
		foreach (var system in systems)
		{
			Container.Bind(system.GetType()).To (system.GetType()).FromInstance (system).AsSingle ();
		}
    }
}