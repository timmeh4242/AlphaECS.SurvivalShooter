using System;
using System.Collections.Generic;
using AlphaECS;
using Zenject;
using UniRx;

namespace AlphaECS
{
//	public class GroupFactory : Factory<Type[], Group>
	public class GroupFactory
    {
		[Inject] protected DiContainer Container = null;

		public Group Create(Type[] types)
		{
			var group = new Group (types);
			Container.Inject (group);
			return group;
		}
//		[Inject] public IEventSystem EventSystem { get; set; }
//        private List<Type> _components;
//        private Predicate<IEntity> _predicate;
//
//        public ReactiveGroupBuilder()
//        {
//            _components = new List<Type>();
//        }
//
//		public ReactiveGroupBuilder Create()
//        {
//            _components = new List<Type>();
//            return this;
//        }
//
//		public ReactiveGroupBuilder WithComponent<T>() where T : class, IComponent
//        {
//            _components.Add(typeof(T));
//            return this;
//        }
//
//		public ReactiveGroupBuilder WithPredicate(Predicate<IEntity> predicate)
//        {
//            _predicate = predicate;
//            return this;
//        }
//
//		public static IReactiveGroup Build(List<Type> components)
//        {
////			var group = new ReactiveGroup(_predicate, _components.ToArray());
//			var group = new ReactiveGroup(components.ToArray());
//			EventSystem.OnEvent<ComponentAddedEvent> ().Where (e => components.Contains (e.Component.GetType ())).Subscribe (_ =>
//			{
//			
//			});
//			return group;
//		}
    }
}