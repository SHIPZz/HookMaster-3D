using System;
using Sirenix.OdinInspector;

namespace CodeBase.Gameplay.GameItems
{
    public class CircleRouletteItem : GameItemAbstract
    {
        public string Id;
        public float PlayTime;

        [Button]
        private void CreateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}