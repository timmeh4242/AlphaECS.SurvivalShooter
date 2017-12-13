using UnityEngine;
using System.Collections;
using AlphaECS.Unity;
using UniRx;
using System;
using UnityEngine.SceneManagement;

namespace AlphaECS.SurvivalShooter
{
	public class SceneSystem : SystemBehaviour
	{
		public override void Initialize (IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory)
		{
			base.Initialize (eventSystem, poolManager, groupFactory);

			EventSystem.OnEvent<LoadSceneEvent> ().Subscribe (e =>
			{
				LoadScene(e.SceneName);
//				UnloadSceneAsync("Level_01").ToObservable()
//					.SelectMany(x => LoadSceneAsync(e.SceneName))
//					.Subscribe(x => 
//					{
//	//					Debug.Log("Loading Complete!");
//					});
			}).AddTo (this);

			EventSystem.OnEvent<UnloadSceneEvent> ().Subscribe (e =>
			{
				StartCoroutine(UnloadSceneAsync(e.SceneName));
			}).AddTo (this);
		}

		void LoadScene(string sceneName)
		{
			SceneManager.LoadScene (sceneName);
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
}
