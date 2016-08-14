using System.Collections.Generic;
using System.Linq;
using EcsRx.Extensions;
using EcsRx.Systems;
using EcsRx.Systems.Executor;
using EcsRx.Unity.Systems;
using Zenject;
using UniRx;
using UnityEngine;

namespace EcsRx.Unity.Installers
{
    /// <summary>
    /// This is for binding AND registering systems
    /// </summary>
    public class AutoRegisterSystemsInstaller : MonoInstaller
    {
        public List<string> SystemNamespaces = new List<string>();

        public override void InstallBindings()
        {
            Container.Bind<ISystem>().To(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();
            Container.Bind(x => x.AllTypes().DerivingFrom<ISystem>().InNamespaces(SystemNamespaces)).AsSingle();

//			Container.Bind<ISystemAlt>().To(x => x.AllTypes().DerivingFrom<ISystemAlt>().InNamespaces(SystemNamespaces)).AsSingle();
//			Container.Bind(x => x.AllTypes().DerivingFrom<ISystemAlt>().InNamespaces(SystemNamespaces)).AsSingle();

            RegisterSystems();
        }

        private void RegisterSystems()
        {
            var allSystems = Container.ResolveAll<ISystem>();
            var systemExecutor = Container.Resolve<ISystemExecutor>();

//			var allAltSystems = Container.ResolveAll<ISystemAlt>();

            var orderedSystems = allSystems
                .OrderByDescending(x => x is ViewResolverSystem)
                .ThenByDescending(x => x is ISetupSystem);
            orderedSystems.ForEachRun(systemExecutor.AddSystem);


//			foreach (var system in allAltSystems)
//			{
////				this.Publish(new ServiceLoaderEvent() {State = ServiceState.Loading, Service = service});
//				system.Setup();
//				var setupAsync = system.SetupAsync();
//				if (setupAsync != null && setupAsync.MoveNext())
//				{
////					yield return StartCoroutine(setupAsync);
//					StartCoroutine(setupAsync);
//				}
////				this.Publish(new ServiceLoaderEvent() {State = ServiceState.Loaded, Service = service});
//			}

//			Observable.TimerFrame (120).First().Subscribe (_ =>
//			{
//				var playerFXSystem = Container.get.FirstOrDefault() as PlayerFXSystem;
//				playerFXSystem.Dispose();
//			});

//			this.Publish(new SystemsLoadedEvent()
//			{
//				Kernel = this
//			});
//
//			_isKernelLoaded = true;
//
//			this.Publish(new KernelLoadedEvent()
//			{
//				Kernel = this
//			});
//			yield return new WaitForEndOfFrame(); //Ensure that everything is bound
//			yield return new WaitForEndOfFrame();
//			this.Publish(new GameReadyEvent());
        }
    }
}