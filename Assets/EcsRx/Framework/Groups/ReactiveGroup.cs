using System;
using System.Collections;
using System.Collections.Generic;
using EcsRx.Entities;
using UniRx;
using EcsRx.Events;
using Zenject;

namespace EcsRx.Groups
{
	public class ReactiveGroup : IReactiveGroup
    {
		[Inject] public IEventSystem EventSystem { get; set; }
		public string Name { get; set; }
		public ReactiveCollection<IEntity> Entities { get; set; }

		public IEnumerable<Type> Components { get; set; }
		public Predicate<IEntity> Predicate { get; set; }

		public ReactiveGroup(params Type[] components)
        {
			Components = components;
			Predicate = null;
        }

		public ReactiveGroup(Predicate<IEntity> predicate, params Type[] components)
        {
			Components = components;
			Predicate = predicate;
        }

		public void Setup ()
		{
			throw new NotImplementedException ();
		}
		public IEnumerator SetupAsync ()
		{
			throw new NotImplementedException ();
		}
    }
}