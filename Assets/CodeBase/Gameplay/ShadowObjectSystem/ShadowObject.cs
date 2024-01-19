using System;
using UnityEngine;

namespace CodeBase.Gameplay.ShadowObjectSystem
{
    [RequireComponent(typeof(UnityEngine.Renderer))]
    public class ShadowObject : MonoBehaviour
    {
        private UnityEngine.Renderer _renderer;
        public Material Material { get; private set; }

        private void Awake()
        {
            _renderer = GetComponent<UnityEngine.Renderer>();
            Material = _renderer.material;
        }
    }
}