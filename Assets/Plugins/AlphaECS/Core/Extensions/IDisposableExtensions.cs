using System;
using System.Collections.Generic;

namespace AlphaECS.Extensions
{
    public static class IDisposableExtensions
    {
        public static void DisposeAll(this IEnumerable<IDisposable> disposables)
        {	
//			disposables.ForEachRun(x => x.Dispose());
		}
    }
}