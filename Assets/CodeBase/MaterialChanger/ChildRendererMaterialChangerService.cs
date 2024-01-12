using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using CodeBase.Data;
using CodeBase.Gameplay.Renderer;
using CodeBase.Services.DataService;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.MaterialChanger
{
    public class ChildRendererMaterialChangerService : RendererMaterialChangerService
    {
        private List<Renderer> _childRenderers;
        private Dictionary<int, List<Material>> _lastRendererMaterials = new();

        public ChildRendererMaterialChangerService(ScriptableObjectDataService scriptableObjectDataService) : base(
            scriptableObjectDataService) { }

        public void Init(float duration, float targetValue, MaterialTypeId materialTypeId, Renderer renderer,
            List<Renderer> childRenderers)
        {
            _childRenderers = childRenderers;
            base.Init(duration, targetValue, materialTypeId, renderer);
        }

        public override void Change()
        {
            foreach (var renderer in _childRenderers)
            {
                Material[] newMaterials = new Material[renderer.materials.Length];

                if (!_lastRendererMaterials.ContainsKey(renderer.GetHashCode()))
                    _lastRendererMaterials[renderer.GetHashCode()] = renderer.GetComponent<RendererMaterialsContainer>().StartMaterials;

                for (int i = 0; i < newMaterials.Length; i++)
                    newMaterials[i] = TargetMaterial;

                DOTween.To(() => 0, value => SetMaterialValue(renderer, value), TargetValue, _duration);
                renderer.materials = newMaterials;
            }

            base.Change();
        }

        public override void SetInitialMaterial()
        {
            base.SetInitialMaterial();

            foreach (var renderer in _childRenderers)
            {
                DOTween.To(() => SavedTargetValue, value => SetMaterialValue(renderer, value), 0f, _duration)
                    .OnComplete(() =>
                    {
                        renderer.materials = _lastRendererMaterials[renderer.GetHashCode()].ToArray();
                    });
            }
        }

        private void SetMaterialValue(Renderer renderer, float x)
        {
            foreach (var material in renderer.materials)
            {
                material.SetFloat(AdvancedDissolveProperties.Cutout.Standard.ids[0].clip, x);
            }
        }
    }
}