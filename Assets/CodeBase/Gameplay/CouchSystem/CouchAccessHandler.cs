using System;
using CodeBase.Enums;
using CodeBase.Services.TriggerObserve;
using UnityEngine;

namespace CodeBase.Gameplay.CouchSystem
{
    public class CouchAccessHandler : MonoBehaviour
    {
        [SerializeField] private TriggerObserver _leftSideObserver;
        [SerializeField] private TriggerObserver _rightSideObserver;
        [SerializeField] private Couch _couch;

        private void OnEnable()
        {
            _leftSideObserver.TriggerExited += OnLeftSideExited;
            _rightSideObserver.TriggerExited += OnRightSideExited;
        }

        private void OnDisable()
        {
            _leftSideObserver.TriggerExited -= OnLeftSideExited;
            _rightSideObserver.TriggerExited -= OnRightSideExited;
        }

        private void OnRightSideExited(Collider obj)
        {
            _couch.SideConditions[SideTypeId.Right] = true;
        }

        private void OnLeftSideExited(Collider client)
        {
            _couch.SideConditions[SideTypeId.Left] = true;
            
        }
    }
}