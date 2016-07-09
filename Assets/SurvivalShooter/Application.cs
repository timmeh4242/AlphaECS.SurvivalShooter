﻿using EcsRx.Unity;
using EcsRx.Unity.Components;
using Zenject;

namespace EcsRx.SurvivalShooter
{
	public class Application : EcsRxApplication
    {
		[Inject]
		public PlayerMovementSystem PlayerMovementSystem { get; private set; }
		protected override void GameStarted ()
		{
			SystemExecutor.AddSystem(PlayerMovementSystem);
		}

//
//		protected override void SetupSystems()
//		{
////			SystemExecutor.AddSystem(PlayerMovementSystem);
//		}
//
//        protected override void SetupEntities()
//        {
//            var defaultPool = PoolManager.GetPool();
//
////            var cubeEntity = defaultPool.CreateEntity();
////            cubeEntity.AddComponent<ViewComponent>();
////			cubeEntity.AddComponent<DamagerComponent>();
//
////            var sphereEntity = defaultPool.CreateEntity();
////            sphereEntity.AddComponent<ViewComponent>();
////            sphereEntity.AddComponent<SphereComponent>();
//        }
    }
}
