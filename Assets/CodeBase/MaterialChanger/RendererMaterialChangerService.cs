using System;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using CodeBase.Data;
using CodeBase.Services.DataService;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.MaterialChanger
{
    public class RendererMaterialChangerService
    {
        private Material[] _lastMaterials;

        protected float SavedTargetValue;
        private Renderer _renderer;
        protected float _duration;
        protected float TargetValue;
        protected Material TargetMaterial;
        protected ScriptableObjectDataService ScriptableObjectDataService;
        private List<Renderer> _childRenderers;

        public bool IsChanging { get; private set; }

        public event Action StartedChanged;
        public event Action Completed;

        public RendererMaterialChangerService(ScriptableObjectDataService scriptableObjectDataService)
        {
            ScriptableObjectDataService = scriptableObjectDataService;
        }

        public void Init(float duration, float targetValue, MaterialTypeId materialTypeId, Renderer renderer)
        {
            _renderer = renderer;
            TargetValue = targetValue;
            TargetMaterial = ScriptableObjectDataService.GetMaterial(materialTypeId);
            _duration = duration;
            SavedTargetValue = targetValue;
        }

        public virtual void SetInitialMaterial()
        {
            DOTween.To(() => SavedTargetValue, SetMaterialValue,
                0f, _duration).OnComplete(() =>
            {
                _renderer.materials = _lastMaterials;
                OnMaterialChanged();
            });
        }

        public virtual void Change()
        {
            Material[] newMaterials = new Material[_renderer.materials.Length];

            _lastMaterials ??= _renderer.materials;
            
            for (int i = 0; i < newMaterials.Length; i++)
                newMaterials[i] = TargetMaterial;

            _renderer.materials = newMaterials;

            StartedChanged?.Invoke();
            IsChanging = true;

            DOTween.To(() => 0, SetMaterialValue, TargetValue, _duration).OnComplete(OnMaterialChanged);
        }

        private void OnMaterialChanged()
        {
            Completed?.Invoke();
        }

        protected void SetMaterialValue(float x)
        {
            foreach (var material in _renderer.materials)
            {
                material.SetFloat(AdvancedDissolveProperties.Cutout.Standard.ids[0].clip, x);
            }
        }
    }
}