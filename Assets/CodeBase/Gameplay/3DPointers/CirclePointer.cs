using UnityEngine;

namespace CodeBase.Gameplay._3DPointers
{
    public class CirclePointer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _pointer;
        [SerializeField] private SpriteRenderer _circle;

        public void Enable()
        {
            _pointer.gameObject.SetActive(true);
            _circle.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _pointer.gameObject.SetActive(false);
            _circle.gameObject.SetActive(false);
        }
    }
}