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

		ReactiveCollection<IEntity> _entities = new ReactiveCollection<IEntity>();
		public ReactiveCollection<IEntity> Entities
		{
			get { return _entities; }
			set { _entities = value; }
		}

		public IEnumerable<Type> Components { get; set; }
		public Predicate<IEntity> Predicate { get; set; }

		protected CompositeDisposable _disposer = new CompositeDisposable();
		public CompositeDisposable Disposer
		{
			get { return _disposer; }
			set { _disposer = value; }
		}

		public Group(params Type[] components)
        {
			Components = components;
			Predicate = null;
        }

//		public Group(Predicate<IEntity> predicate, params Type[] components)
//        {
//			Components = components;
//			Predicate = predicate;
//        }

		[Inject]
		public void Setup(IEventSystem eventSystem, IPoolManager poolManager)
		{
			EventSystem = eventSystem;
			EntityPool = poolManager.GetPool ();

			foreach (IEntity entity in EntityPool.Entities)
			{
				if (entity.HasComponents (Components.ToArray ()))
				{
					AddEntity (entity);
				}
			}

			EventSystem.OnEvent<EntityAddedEvent> ().Where (_ => _.Entity.HasComponents (Components.ToArray())).Subscribe (_ =>
			{
				AddEntity(_.Entity);
			}).AddTo(this);

			EventSystem.OnEvent<EntityRemovedEvent> ().Where (_ => Entities.Contains(_.Entity)).Subscribe (_ =>
			{
				RemoveEntity(_.Entity);
			}).AddTo(this);

			EventSystem.OnEvent<ComponentAddedEvent> ().Where (_ => _.Entity.HasComponents (Components.ToArray()) && Entities.Contains(_.Entity) == false).Subscribe (_ =>
			{
				AddEntity(_.Entity);
			}).AddTo(this);

			EventSystem.OnEvent<ComponentRemovedEvent> ().Where (_ => Components.Contains(_.Component.GetType()) && Entities.Contains(_.Entity)).Subscribe (_ =>
			{
				RemoveEntity(_.Entity);
			}).AddTo(this);
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
    }
}