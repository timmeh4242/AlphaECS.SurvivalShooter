namespace EcsRx.Systems
{
	using EcsRx.Entities;
	using EcsRx.Groups;
	using EcsRx.Events;
	using System.Collections;
	using Zenject;

	public interface IReactiveSystem
	{
		IEventSystem EventSystem { get; set; }
		void Setup();
		IEnumerator SetupAsync();
	}
}