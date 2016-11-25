using UnityEngine;

namespace AlphaECS
{
    public static class ViewComponentExtensions
    {
        public static void DestroyView(this ViewComponent viewComponent, float delay = 0.0f)
        {
            Object.Destroy(viewComponent.View, delay);
        }
    }
}