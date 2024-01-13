using UnityEngine;

namespace CodeBase.SO.Settings
{
    [CreateAssetMenu(fileName = "SettingSO", menuName = "Gameplay/SoundSO/SO")]
    public class SettingSO : ScriptableObject
    {
        public float MusicVolume = -20f;
        public float MainVolume = -10f;
    }
}