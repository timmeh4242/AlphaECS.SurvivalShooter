using UnityEngine;
using System.Collections;
using EcsRx.Unity;
using UniRx;
using System;
using UnityEngine.SceneManagement;

public class SceneManagementSystem : SystemBehaviour
{
	public override void Setup ()
	{
		base.Setup ();

		EventSystem.OnEvent<LoadSceneEvent> ().Subscribe (e =>
		{
			Observable.Timer(TimeSpan.FromSeconds(0f))
				.SelectMany(x => UnloadSceneAsync("Level_01"))
				.SelectMany(x => LoadSceneAsync(e.SceneName))
				.Subscribe(x => 
				{
//					Debug.Log("Loading Complete!");
				});
		}).AddTo (this);

		EventSystem.OnEvent<UnloadSceneEvent> ().Subscribe (e =>
		{
			StartCoroutine(UnloadSceneAsync(e.SceneName));
		}).AddTo (this);
	}

	IEnumerator LoadSceneAsync(string sceneName)
	{
		var asyncOperation = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		while (!asyncOperation.isDone)
		{
			yield return null;
		}
		yield return null;
	}

	IEnumerator UnloadSceneAsync(string sceneName)
	{
		var isUnloaded = SceneManager.UnloadScene (sceneName);
		while (!isUnloaded)
		{
			yield return null;
		}
		yield return null;
	}
}
