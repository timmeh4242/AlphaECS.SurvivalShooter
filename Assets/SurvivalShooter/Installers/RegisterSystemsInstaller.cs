using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using EcsRx.Unity.Systems;
using Zenject;

namespace EcsRx.SurvivalShooter
{
	public class RegisterSystemsInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<DamageSystem> ().AsSingle ();
			Container.Bind<EnemyFXSystem> ().AsSingle ();
			Container.Bind<HealthSystem> ().AsSingle ();
			Container.Bind<MeleeSystem> ().AsSingle ();
			Container.Bind<NavMeshMovementSystem> ().AsSingle ();
			Container.Bind<PlayerFXSystem> ().AsSingle ();
			Container.Bind<PlayerMovementSystem> ().AsSingle ();
			Container.Bind<ShootingSystem> ().AsSingle ();

			RegisterSystems();
		}

		private void RegisterSystems()
		{
			var allSystems = Container.ResolveAll<ISystem>();
			var systemExecutor = Container.Resolve<ISystemExecutor>();

			var orderedSystems = allSystems
				.OrderByDescending(x => x is ViewResolverSystem)
				.ThenByDescending(x => x is ISetupSystem);
			orderedSystems.ForEachRun(systemExecutor.AddSystem);
		}
	}
}