#if !NET_LEGACY
using System;
#else
using UniRx;
#endif

namespace AlphaECS
{
    public interface IEventSystem
    {
        void Publish<T>(T message);
        IObservable<T> OnEvent<T>();
    }
}