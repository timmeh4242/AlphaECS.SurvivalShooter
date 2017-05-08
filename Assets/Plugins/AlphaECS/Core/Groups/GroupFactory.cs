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

//		protected Dictionary<HashSet<Type>, Dictionary<HashSet<List<Func<IEntity, ReactiveProperty<bool>>>>, Group> Groups = new Dictionary<HashSet<List<Func<IEntity, ReactiveProperty<bool>>>>, Group>();
		protected Dictionary<HashSet<Type>, Group> Groups = new Dictionary<HashSet<Type>, Group>();

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
			var hashSet = new HashSet<Type> (types);
			if (Groups.ContainsKey (hashSet))
			{
				UnityEngine.Debug.Log ("group cached!");
				return Groups [hashSet];
			}

			var group = new Group (types, predicates);
			Container.Inject (group);

			this.types = null;
			this.predicates.Clear();
			Groups.Add (hashSet, group);
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