using UnityEngine;
using Zenject;

namespace CodeBase.Cheats
{
    public class Cheat : ITickable
    {
        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }
    }
}