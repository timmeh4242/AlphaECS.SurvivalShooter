using UnityEngine;
using System.Collections;
using AlphaECS.Unity;

namespace AlphaECS.SurvivalShooter
{
	public class SpawnerComponent : ComponentBehaviour
	{
		public GameObject Prefab;
		public float SpawnTime;
	}
}
