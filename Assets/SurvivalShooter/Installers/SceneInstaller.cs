using UnityEngine;
using Zenject;
using EcsRx.Systems;
using System.Linq;
using System.Collections.Generic;

public class SceneInstaller : MonoInstaller
{
	public List<string> SystemNamespaces = new List<string>();

    public override void InstallBindings()
    {
		var systemTypes = GetComponentsInChildren<IReactiveSystem> ().Select (x => x.GetType ());
		Container.Bind (systemTypes).AsSingle ();
    }
}