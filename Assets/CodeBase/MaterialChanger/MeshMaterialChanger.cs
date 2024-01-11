using System;
using System.Linq;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.MaterialChanger
{
    [RequireComponent(typeof(MeshRenderer))]
    public class MeshMaterialChanger : MonoBehaviour, IMaterialChanger
    {
        [SerializeField] protected float Duration = 1.5f;
        [SerializeField] protected float TargetValue = 1f;
        [SerializeField] protected Material TargetMaterial;

        private MeshRenderer _meshRenderer;
        private Material[] _lastMaterials;

        private float _savedTargetValue;

        public bool IsChanging { get; private set; }

        public event Action StartedChanged;
        public event Action Completed;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        [Button]
        public virtual void SetInitialMaterial()
        {
            DOTween.To(() => TargetValue, SetMaterialValue,
                0f, Duration).OnComplete(() =>
            {
                _meshRenderer.materials = _lastMaterials;
                OnMaterialChanged();
            });
        }

        [Button]
        public virtual void Change()
        {
            Material[] newMaterials = new Material[_meshRenderer.materials.Length];

            _lastMaterials = _meshRenderer.materials;

            for (int i = 0; i < newMaterials.Length; i++)
                newMaterials[i] = TargetMaterial;

            _meshRenderer.materials = newMaterials;

            StartedChanged?.Invoke();
            IsChanging = true;

            DOTween.To(() => 0, SetMaterialValue, TargetValue, Duration).OnComplete(OnMaterialChanged);
        }

        private void OnMaterialChanged()
        {
            Completed?.Invoke();
        }

        private void SetMaterialValue(float x)
        {
            foreach (var material in _meshRenderer.materials)
            {
                material.SetFloat(AdvancedDissolveProperties.Cutout.Standard.ids[0].clip, x);
            }
        }
    }
}