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
    }
}