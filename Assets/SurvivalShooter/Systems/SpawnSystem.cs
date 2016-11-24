using UniRx;
using UnityEngine;
using AlphaECS.Unity;
using Zenject;
using System;
using System.Collections;
using AlphaECS;

namespace AlphaECS.SurvivalShooter
{
	public class SpawnSystem : SystemBehaviour 
	{
		// TODO remove this in favor of a factory...
		[Inject] DiContainer Container { get; set; }

		public override void Setup ()
		{
			base.Setup ();

			var group = GroupFactory.Create (new Type[]{ typeof(SpawnerComponent) });
			group.Entities.ObserveAdd ().Select (x => x.Value).StartWith (group.Entities).Subscribe (entity =>
			{
				var spawner = entity.GetComponent<SpawnerComponent>();
				var delay = TimeSpan.FromSeconds(0f);
				var interval = TimeSpan.FromSeconds(spawner.SpawnTime);
				Observable.Timer(delay, interval).Subscribe(_ =>
				{
//			        if(playerHealth.currentHealth <= 0f)
//			        {
//			            return;
//			        }
				
//					Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
		
					var instance = Container.InstantiatePrefab (spawner.Prefab);
					instance.transform.SetParent(spawner.transform);
					instance.transform.position = spawner.transform.position;
					instance.transform.rotation = spawner.transform.rotation;
				}).AddTo(spawner);
			}).AddTo (this);
		}
	}
}
