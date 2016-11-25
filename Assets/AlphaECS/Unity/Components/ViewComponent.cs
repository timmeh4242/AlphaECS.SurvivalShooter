using AlphaECS;
using UnityEngine;

namespace AlphaECS
{
    public class ViewComponent : IComponent
    {
        public bool DestroyWithView { get; set; }
        public GameObject View { get; set; }
    }
}