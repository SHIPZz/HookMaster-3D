using System;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.MaterialChanger
{
    public class SkinnedMeshMaterialChanger : MonoBehaviour, IMaterialChanger
    {
        [SerializeField] protected float Duration = 1.5f;
        [SerializeField] protected float TargetValue = 1f;
        [SerializeField] protected Material TargetMaterial;

        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private Material[] _lastMaterials;

        private float _savedTargetValue;

        public bool IsChanging { get; private set; }

        public event Action StartedChanged;
        public event Action Completed;

        private void Awake()
        {
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        [Button]
        public virtual void SetInitialMaterial()
        {
            DOTween.To(() => TargetValue, SetMaterialValue,
                0f, Duration).OnComplete(() =>
            {
                _skinnedMeshRenderer.materials = _lastMaterials;
                OnMaterialChanged();
            });
        }

        [Button]
        public virtual void Change()
        {
            Material[] newMaterials = new Material[_skinnedMeshRenderer.materials.Length];

            _lastMaterials = _skinnedMeshRenderer.materials;

            for (int i = 0; i < newMaterials.Length; i++)
                newMaterials[i] = TargetMaterial;

            _skinnedMeshRenderer.materials = newMaterials;

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
            foreach (var material in _skinnedMeshRenderer.materials)
            {
                material.SetFloat(AdvancedDissolveProperties.Cutout.Standard.ids[0].clip, x);
            }
        }
    }
}