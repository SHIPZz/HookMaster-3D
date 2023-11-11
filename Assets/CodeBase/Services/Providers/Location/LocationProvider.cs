using System.Collections.Generic;
using CodeBase.Enums;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CodeBase.Services.Providers.Location
{
    public class LocationProvider : SerializedMonoBehaviour
    {
        [OdinSerialize] public Dictionary<LocationTypeId, Transform> Locations;
    }
}