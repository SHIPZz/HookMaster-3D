using System;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using CodeBase.Data;
using CodeBase.Gameplay.Renderer;
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
        protected MaterialStaticDataService _materialStaticDataService;

        public bool IsChanging { get; private set; }

        public RendererMaterialChangerService(MaterialStaticDataService materialStaticDataService)
        {
            _materialStaticDataService = materialStaticDataService;
        }

        public void Init(float duration, float targetValue, MaterialTypeId materialTypeId, Renderer renderer)
        {
            _renderer = renderer;
            TargetValue = targetValue;
            TargetMaterial = _materialStaticDataService.GetMaterial(materialTypeId);
            _duration = duration;
            SavedTargetValue = targetValue;
        }

        public virtual void SetInitialMaterial()
        {
            DOTween.To(() => SavedTargetValue, SetMaterialValue,
                0f, _duration).OnComplete(() => _renderer.materials = _lastMaterials);
        }

        public virtual void Change()
        {
            Material[] newMaterials = new Material[_renderer.materials.Length];

            _lastMaterials ??= _renderer.GetComponent<RendererMaterialsContainer>().StartMaterials.ToArray();

            for (int i = 0; i < newMaterials.Length; i++)
                newMaterials[i] = TargetMaterial;

            _renderer.materials = newMaterials;

            DOTween.To(() => 0, SetMaterialValue, TargetValue, _duration);
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