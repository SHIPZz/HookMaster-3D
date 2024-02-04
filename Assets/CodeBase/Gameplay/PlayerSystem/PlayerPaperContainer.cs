using System;
using System.Collections.Generic;
using CodeBase.Gameplay.PaperSystem;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerPaperContainer : MonoBehaviour
    {
        private Stack<Paper> _papers = new();

        public IReadOnlyCollection<Paper> Papers => _papers;

        public event Action Cleared;
        
        public void Push(Paper paper) => _papers.Push(paper);

        public void Clear()
        {
            _papers.Clear();
            Cleared?.Invoke();
        }

        public void Pop()
        {
            _papers.Pop();
        }
    }
}