using System;
using UnityEngine;

namespace CodeBase.Gameplay.ResourceItem
{
    public interface IResourceCollector
    {
        event Action<IResourceCollector, IResource> ResourceDetected; 
        Transform Anchor { get; }
        Transform ControlPoint { get; }
    }
}