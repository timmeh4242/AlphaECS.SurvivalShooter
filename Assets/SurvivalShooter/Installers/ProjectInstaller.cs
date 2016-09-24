using UnityEngine;
using Zenject;
using EcsRx;
using System.Linq;
using System.Collections.Generic;
using EcsRx.Unity;
using UnityEngine.SceneManagement;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
		var Kernel = "Kernel";
		SceneManager.LoadScene (Kernel, LoadSceneMode.Additive);
		var kernelScene = SceneManager.GetSceneByName (Kernel);
		SceneManager.MoveGameObjectToScene (ProjectContext.Instance.gameObject, kernelScene);

		var systems = GetComponentsInChildren<SystemBehaviour> ();
		foreach (var system in systems)
		{
			Container.Bind(system.GetType()).FromInstance (system).AsSingle ();
		}
    }
}