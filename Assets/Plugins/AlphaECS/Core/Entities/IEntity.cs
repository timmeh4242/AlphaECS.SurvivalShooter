using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaECS
{
	public interface IEntity : IDisposable
    {
        int Id { get; }
		IEnumerable<object> Components { get; }

		object AddComponent(object component);
		T AddComponent<T> () where T : class, new(); 
		void RemoveComponent(object component);
        void RemoveComponent<T>() where T : class;
        void RemoveAllComponents();
        T GetComponent<T>() where T : class;

        bool HasComponent<T>() where T : class;
        bool HasComponents(params Type[] component);
    }
}
