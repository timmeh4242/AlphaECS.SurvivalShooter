using UnityEngine;
using Zenject;
using AlphaECS.SurvivalShooter;

public class GroupsInstaller : MonoInstaller<GroupsInstaller>
{
    public override void InstallBindings()
    {
		Container.Bind<DeadEntities>().To<DeadEntities>().AsSingle();
    }
}