﻿using System;
using System.Collections.Generic;
using CodeBase.Gameplay.PaperSystem;
using UnityEngine;

namespace CodeBase.Gameplay.PlayerSystem
{
    public class PlayerPaperContainer : MonoBehaviour
    {
        private Stack<Paper> _papers = new();

        public IReadOnlyCollection<Paper> Papers => _papers;

        public bool HasPapers => _papers.Count > 0;

        public event Action Cleared;
        public event Action Removed;
        public event Action Added;

        public void Push(Paper paper)
        {
            _papers.Push(paper);
            Added?.Invoke();
        }

        public void Clear()
        {
            _papers.Clear();
            Cleared?.Invoke();
        }

        public Paper Pop()
        {
            var paper = _papers.Pop();

            if (_papers.Count == 0)
                Clear();

            Removed?.Invoke();
            return paper;
        }

        public Paper Peek()
        {
            if (_papers.TryPeek(out Paper paper))
                return paper;

            return null;
        }
    }
}