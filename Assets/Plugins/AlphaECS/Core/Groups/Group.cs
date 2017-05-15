using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Zenject;
using UnityEngine;
using System.Linq;

namespace AlphaECS
{
	public class Group : IGroup, IDisposableContainer, IDisposable
    {
		public IEventSystem EventSystem { get; set; }
		public IPool EntityPool { get; set; }

		public string Name { get; set; }

		public ReactiveCollection<IEntity> cachedEntities = new ReactiveCollection<IEntity>();

		ReactiveCollection<IEntity> _entities = new ReactiveCollection<IEntity>();
		public ReactiveCollection<IEntity> Entities
		{
			get { return _entities; }
			set { _entities = value; }
		}

		public IEnumerable<Type> Components { get; set; }

		protected List<Func<IEntity, ReactiveProperty<bool>>> _predicates = new List<Func<IEntity, ReactiveProperty<bool>>> ();
		public List<Func<IEntity, ReactiveProperty<bool>>> Predicates
		{
			get { return _predicates; }
			private set { _predicates = value; }
		}

		protected CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			private set { _disposer = value; }
		}

		public Group() { }
		public Group(Type[] components, List<Func<IEntity, ReactiveProperty<bool>>> predicates)
        {
			Components = components;

			foreach (var predicate in predicates)
			{
				Predicates.Add (predicate);
			}
        }

		[Inject]
		public virtual void Setup(IEventSystem eventSystem, IPoolManager poolManager)
		{
			EventSystem = eventSystem;
			EntityPool = poolManager.GetPool ();

			cachedEntities.ObserveAdd ().Select (x => x.Value).Subscribe (entity =>
			{
				if(Predicates.Count == 0)
				{
					PreAdd(entity);
					AddEntity(entity);
					return;
				}
				
				var bools = new List<ReactiveProperty<bool>>();
				foreach (var predicate in Predicates)
				{
					bools.Add(predicate.Invoke(entity));
				}
				var onLatest = Observable.CombineLatest(bools.ToArray());
				onLatest.DistinctUntilChanged().Subscribe(values =>
				{
					if(values.All(value => value == true))
					{
						PreAdd(entity);
						AddEntity(entity);
					}
					else
					{
						PreRemove(entity);
						RemoveEntity(entity);
					}
				}).AddTo (this.Disposer);;
			}).AddTo (this.Disposer);

			cachedEntities.ObserveRemove ().Select (x => x.Value).Subscribe (entity =>
			{
				PreRemove(entity);
				RemoveEntity(entity);
			}).AddTo (this.Disposer);

			foreach (IEntity entity in EntityPool.Entities)
			{
				if (entity.HasComponents (Components.ToArray ()))
				{
					cachedEntities.Add(entity);
				}
			}

			EventSystem.OnEvent<EntityAddedEvent> ().Where (_ => _.Entity.HasComponents (Components.ToArray()) && cachedEntities.Contains(_.Entity) == false).Subscribe (_ =>
			{ cachedEntities.Add(_.Entity); }).AddTo(this);

			EventSystem.OnEvent<EntityRemovedEvent> ().Where (_ => cachedEntities.Contains(_.Entity)).Subscribe (_ =>
			{ cachedEntities.Remove(_.Entity); }).AddTo(this);

			EventSystem.OnEvent<ComponentAddedEvent> ().Where (_ => _.Entity.HasComponents (Components.ToArray()) && cachedEntities.Contains(_.Entity) == false).Subscribe (_ =>
			{ cachedEntities.Add(_.Entity); }).AddTo(this);

			EventSystem.OnEvent<ComponentRemovedEvent> ().Where (_ => Components.Contains(_.Component.GetType()) && cachedEntities.Contains(_.Entity)).Subscribe (_ =>
			{ cachedEntities.Remove(_.Entity); }).AddTo(this);
		}

		void AddEntity(IEntity entity)
		{
			Entities.Add (entity);
		}

		void RemoveEntity(IEntity entity)
		{
			Entities.Remove (entity);
		}
			
		public void Dispose ()
		{
			Disposer.Dispose ();
		}

		protected virtual void PreAdd(IEntity entity) {}
		protected virtual void PreRemove(IEntity entity) {}
    }
}