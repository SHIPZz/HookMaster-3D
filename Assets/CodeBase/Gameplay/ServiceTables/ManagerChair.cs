using CodeBase.Animations;
using UnityEngine;

namespace CodeBase.Gameplay.ServiceTables
{
    public class ManagerChair : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _appearanceEffect;
        [SerializeField] private TransformScaleAnim _transformScale;

        public void Enable()
        {
            gameObject.SetActive(true);
            _appearanceEffect.Play();
            _transformScale.ToScale();
        }
    }
}