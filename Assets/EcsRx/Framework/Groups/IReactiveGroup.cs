using System;
using System.Collections.Generic;
using EcsRx.Entities;
using UniRx;
using System.Collections;
using EcsRx.Events;

namespace EcsRx.Groups
{
    public interface IReactiveGroup
    {
		IEventSystem EventSystem { get; set; }
		string Name { get; set; }
		ReactiveCollection<IEntity> Entities { get; set; }

		IEnumerable<Type> Components { get; set; }
		Predicate<IEntity> Predicate { get; }

//		bool Match ();
		void Setup ();
		IEnumerator SetupAsync();
//        Predicate<IEntity> TargettedEntities { get; }
    }
}