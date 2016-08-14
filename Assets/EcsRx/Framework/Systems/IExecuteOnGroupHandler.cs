using EcsRx.Entities;
using EcsRx.Groups;
using UniRx;

namespace EcsRx.Systems
{
    public interface IExecuteOnGroupHandler : ISystem
    {
		IObservable<IGroup> GetGroup ();
		void ExecuteOnGroup (IGroup group);
    }
}