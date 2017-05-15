using UnityEngine;
using Zenject;
using AlphaECS;
using System.Linq;
using System.Collections.Generic;
using AlphaECS.Unity;
using UnityEngine.SceneManagement;
using UniRx;

public class ProjectInstaller : MonoInstaller
{
	List<GameObject> KernelObjects = new List<GameObject> ();

	public override void InstallBindings()
    {
		var resources = Resources.LoadAll ("Kernel");
		foreach(var resource in resources)
		{
			var go = (GameObject)Instantiate (resource);
			DontDestroyOnLoad (go);
			KernelObjects.Add (go);
			var systems = go.GetComponentsInChildren<SystemBehaviour> ();
			foreach (var system in systems)
			{
				Container.Bind(system.GetType()).FromInstance (system).AsSingle ();
			}
		}

		/* zenject will throw a warning here
		* we can safely ignore this as we're using our "setup" method on these kernel systems as a di constructor only
		* and the dependencies which we're injecting are in the framework scope and have already been bound to the container with AlphaECSInstaller */
		foreach(var go in KernelObjects)
		{
			Container.InjectGameObject (go);
		}
    }
}