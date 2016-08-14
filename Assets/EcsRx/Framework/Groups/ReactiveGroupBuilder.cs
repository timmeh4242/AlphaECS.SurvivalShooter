using System;
using System.Collections.Generic;
using EcsRx.Components;
using EcsRx.Entities;
using Zenject;
using EcsRx.Events;
using UniRx;

namespace EcsRx.Groups
{
    public static class ReactiveGroupBuilder
    {
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