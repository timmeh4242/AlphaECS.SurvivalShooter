using UniRx;

namespace AlphaECS
{
    public static class IObservableExtensions
    {
        public static IObservable<Unit> AsTrigger<T>(this IObservable<T> observable)
        { return observable.Select(x => Unit.Default); } 
    }
}