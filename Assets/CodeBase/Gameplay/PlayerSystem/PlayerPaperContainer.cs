using System.Collections.Generic;
using CodeBase.Gameplay.PaperSystem;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerPaperContainer : MonoBehaviour
    {
        private List<Paper> _papers = new();

        public IReadOnlyList<Paper> Papers => _papers;

        public void Add(Paper paper) => _papers.Add(paper);

        public void Remove(Paper paper) => _papers.Remove(paper);

        public void Clear()
        {
            _papers.Clear();
        }
    }
}