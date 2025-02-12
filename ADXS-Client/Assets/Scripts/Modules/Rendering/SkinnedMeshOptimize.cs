using System;
using UnityEngine;

namespace Modules.Rendering
{
    public class SkinnedMeshOptimize : MonoBehaviour
    {
        public SkinnedMeshRenderer meshRenderer;
        public Action<bool> OnVisible;

        private void Awake()
        {
            meshRenderer = GetComponent<SkinnedMeshRenderer>();
        }


        void OnBecameVisible()
        {
            print(gameObject.name + " is visible");
            OnVisible?.Invoke(true);
        }

        void OnBecameInvisible()
        {
            print(gameObject.name + " is visible");
            OnVisible?.Invoke(false);
        }
    }
}