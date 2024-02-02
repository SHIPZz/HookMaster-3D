using System.Collections.Generic;
using CodeBase.Gameplay.PaperSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Gameplay.TableSystem
{
    public class PaperTable : MonoBehaviour
    {
        [field: SerializeField] public Transform PaperPosition { get; private set; }
        [field: SerializeField] public Transform PaperFinishedPosition { get; private set; }
        [field: SerializeField] public Vector3 Offset { get; private set; } = new Vector3(0, 0.06f, 0);

        public List<Paper> PapersOnTable = new();
        
        public async void Add(Paper paper)
        {
            PapersOnTable.Add(paper);
            await UniTask.WaitForSeconds(2f);
            paper.transform.SetParent(PaperFinishedPosition);
            paper.transform.DOLocalJump(Vector3.zero, 1f, 1, 1f);
            paper.transform.DOScale(Vector3.zero, 1.5f);
        }
    }
}