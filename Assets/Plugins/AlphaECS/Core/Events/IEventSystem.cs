using UniRx;
using System;

namespace AlphaECS
{
    public interface IEventSystem
    {
        void Publish<T>(T message);
        IObservable<T> OnEvent<T>();
    }
}