using System;
using Sirenix.OdinInspector;

namespace CodeBase.Gameplay.GameItems
{
    public class CircleRouletteItem : GameItemAbstract
    {
        public string Id;
        public int PlayTime;
        public int MinWinValue;
        public int MaxWinValue;

        [Button]
        private void CreateId()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}