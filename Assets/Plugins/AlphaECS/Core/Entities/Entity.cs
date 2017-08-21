using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlphaECS;
using UniRx;

namespace AlphaECS
{
	public class Entity : IEntity
    {
		private readonly Dictionary<Type, object> _components;

        public IEventSystem EventSystem { get; private set; }

		public string Id { get; private set; }
		public IEnumerable<object> Components { get { return _components.Values; } }

        public Entity(string id, IEventSystem eventSystem)
        {
            Id = id;
            EventSystem = eventSystem;
			_components = new Dictionary<Type, object>();
        }

		public object AddComponent(object component)
        {
			//TODO not sure this should be silently returning this way... 
			//... ideally we should be returning the component and able to check for null elsewhere
			if(_components.ContainsKey(component.GetType()))
			{
//				throw new Exception(string.Format("Entity already contains a component of type {0}. Returning pre-existing component.", component.GetType().Name));
				return _components [component.GetType ()];
			}

            _components.Add(component.GetType(), component);
            EventSystem.Publish(new ComponentAddedEvent(this, component));
			return component;
        }

		public T AddComponent<T>() where T : class, new()
		{ return (T)AddComponent(new T()); }

		public void RemoveComponent(object component)
        {
            if(!_components.ContainsKey(component.GetType())) { return; }

            if(component is IDisposable)
            { (component as IDisposable).Dispose(); }

            _components.Remove(component.GetType());
            EventSystem.Publish(new ComponentRemovedEvent(this, component));
        }

		public void RemoveComponent<T>() where T : class
        {
            if(!HasComponent<T>()) { return; }

            var component = GetComponent<T>();
            RemoveComponent(component);
        }

        public void RemoveAllComponents()
        {
            var components = Components.ToArray();
            components.ForEachRun(RemoveComponent);
        }

		public bool HasComponent<T>() where T : class
        { return _components.ContainsKey(typeof(T)); }

		public bool HasComponent(Type componentType)
		{
			return _components.ContainsKey(componentType);
		}

        public bool HasComponents(params Type[] componentTypes)
        {
            if(_components.Count == 0)
            { return false; }

            return componentTypes.All(x => _components.ContainsKey(x));
        }

		public T GetComponent<T>() where T : class
        { return _components[typeof(T)] as T; }

		public object GetComponent(Type type)
		{
			if (_components.ContainsKey (type))
			{
				return _components [type];
			}
			else
			{
				return null;
			}
		}

		public void Dispose()
		{ RemoveAllComponents(); }
    }
}
