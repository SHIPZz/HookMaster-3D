using CodeBase.Enums;
using UnityEngine;
using Zenject;

namespace CodeBase.Installers.ScriptableObject
{
    [CreateAssetMenu(fileName = nameof(ColorSOInstaller), menuName = "Gameplay/Installers/ColorSO")]
    public class ColorSOInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private Color _moneyColor;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_moneyColor).WithId(ColorTypeId.Money);
        }
    }
}