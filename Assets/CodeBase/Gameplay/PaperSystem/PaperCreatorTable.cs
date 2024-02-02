using System;
using System.Collections.Generic;
using CodeBase.Gameplay.ObjectCreatorSystem;
using CodeBase.Gameplay.PlayerSystem;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.PaperSystem
{
    public class PaperCreatorTable : MonoBehaviour
    {
        [SerializeField] private ResourceCreator _resourceCreator;
        [SerializeField] private TriggerObserver _triggerObserver;

        private List<Paper> _papers = new();

        private void OnEnable()
        {
            for (int i = 0; i < 5; i++)
            {
                _resourceCreator.Create();
            }
        }
        //
        // private void OnDisable()
        // {
        //     _triggerObserver.TriggerEntered -= PlayerEntered;
        // }
        //
        // private void PlayerEntered(Collider player)
        // {
        //    var playerPaperContainer =  player.GetComponent<PlayerPaperContainer>();
        //
        //    IReadOnlyList<Paper> playerPapers = playerPaperContainer.Papers;
        //
        //    foreach (Paper paper in playerPapers)
        //    {
        //        paper.do
        //    }
        //    
        

        private void StopMove(Collision obj)
        {
            

        }
    }
}