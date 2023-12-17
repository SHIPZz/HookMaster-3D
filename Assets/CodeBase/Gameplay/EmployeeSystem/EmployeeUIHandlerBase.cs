using UnityEngine;

namespace CodeBase.Gameplay.EmployeeSystem
{
    public abstract class EmployeeUIHandlerBase : MonoBehaviour
    {
        [SerializeField] protected Employee Employee;
        [SerializeField] protected float DownPositionY = 1f;
        [SerializeField] protected float UpPositionY = 3f;
        [SerializeField] protected float FadeInDuration = 0.5f;
        [SerializeField] protected float FadeOutDuration = 0.5f;
        [SerializeField] protected float DownDuration = 0.5f;
        [SerializeField] protected float UpDuration = 0.25f;
    }
}