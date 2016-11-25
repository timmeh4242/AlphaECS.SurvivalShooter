using UniRx;

namespace AlphaECS
{
    public interface IEventSystem
    {
        void Publish<T>(T message);
        IObservable<T> OnEvent<T>();
    }
}