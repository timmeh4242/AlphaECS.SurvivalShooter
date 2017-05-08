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

		private IGroup group;
		private Type[] types;
		private List<Func<IEntity, ReactiveProperty<bool>>> predicates = new List<Func<IEntity, ReactiveProperty<bool>>> ();

		public Group Create(Type[] types)
		{
			this.types = types;
			return this.Create ();
		}

		public Group Create()
		{
			var group = new Group (types, predicates);
			Container.Inject (group);
			this.types = null;
			this.predicates.Clear();
			return group;
		}

		public GroupFactory AddTypes(Type[] types)
		{
			this.types = types;
			return this;
		}

		public GroupFactory WithPredicate(Func<IEntity, ReactiveProperty<bool>> predicate)
		{
			this.predicates.Add (predicate);
			return this;
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