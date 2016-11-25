using System.Collections.Generic;
using System.Linq;
using AlphaECS;
//using AlphaECS.Systems.Executor;

namespace AlphaECS.Extensions
{
    public static class IListExtensions
    {
//        public static IEnumerable<SubscriptionToken> GetTokensFor(this IList<SubscriptionToken> subscriptionTokens, IEntity entity)
//        { return subscriptionTokens.Where(x => x.AssociatedObject == entity); }

        public static void RemoveAll<T>(this IList<T> list, IEnumerable<T> elementsToRemove)
        {
//            elementsToRemove.ToArray()
//                .ForEachRun(x => list.Remove(x));
        }
    }
}