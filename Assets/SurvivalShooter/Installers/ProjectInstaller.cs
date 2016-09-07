using UnityEngine;
using Zenject;
using EcsRx;
using System.Linq;
using System.Collections.Generic;

public class ProjectInstaller : MonoInstaller
{
//	public List<string> SystemNamespaces = new List<string>();

    public override void InstallBindings()
    {
		var systemTypes = GetComponentsInChildren<ISystem> ().Select (x => x.GetType ());
		Container.Bind (systemTypes).AsSingle ();
    }
}