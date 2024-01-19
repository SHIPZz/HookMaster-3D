using DG.Tweening;
using UnityEngine;

namespace CodeBase.Animations
{
    public class MaterialFadeAnimService 
    {
        public void FadeOut(Material material, float duration, float value)
        {
            material.DOFade(value, duration);
        }
    }
}