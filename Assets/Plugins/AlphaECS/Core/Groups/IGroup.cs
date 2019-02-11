using System;
using System.Collections.Generic;
using AlphaECS;
using UniRx;
using System.Collections;

namespace AlphaECS
{
	public interface IGroup : IEnumerable<IEntity>
    {
		IEventSystem EventSystem { get; set; }
		IPool EntityPool { get; set; }
		string Name { get; set; }
		ReactiveCollection<IEntity> Entities { get; set; }

        IEntity this[int index] { get; }

        HashSet<Type> Components { get; set; }
        List<Func<IEntity, IReadOnlyReactiveProperty<bool>>> Predicates { get; }

        IObservable<IEntity> OnAdd();
        IObservable<IEntity> OnRemove();

        IObservable<IEntity> OnPreAdd();
        IObservable<IEntity> OnPreRemove();
    }
}