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
	public const string KernelScene = "Kernel";
	List<GameObject> KernelObjects = new List<GameObject> ();

	public override void InstallBindings()
    {
    	// this is a dummy scene we load in first because unity doesn't let you unload all scenes
		SceneManager.LoadScene (KernelScene, LoadSceneMode.Additive);
		var kernelScene = SceneManager.GetSceneByName (KernelScene);

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

		foreach(var go in KernelObjects)
		{
			Container.InjectGameObject (go);
		}
    }
}