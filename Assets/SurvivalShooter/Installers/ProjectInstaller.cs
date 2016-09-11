using UnityEngine;
using Zenject;
using EcsRx;
using System.Linq;
using System.Collections.Generic;
using EcsRx.Unity;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		var systemTypes = GetComponentsInChildren<SystemBehaviour> ();
		foreach (var system in systemTypes)
		{
			Container.Bind(system.GetType()).To (system.GetType()).FromInstance (system).AsSingle ();
		}
    }
}