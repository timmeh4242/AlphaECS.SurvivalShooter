using System.Collections;

namespace AlphaECS
{
	public interface ISystem
	{
		IEventSystem EventSystem { get; set; }
		IPoolManager PoolManager { get; set; }
		void Setup(IEventSystem eventSystem, IPoolManager poolManager, GroupFactory groupFactory);
	}
}