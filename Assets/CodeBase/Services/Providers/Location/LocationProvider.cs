using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Services.Providers.Location
{
    public class LocationProvider : MonoBehaviour
    {
        public Transform PlayerSpawnPoint;
        public Transform EmployeeSpawnPoint;
        public Transform UIParent;
        public Transform EffectParent;
        public Transform AudioParent;
        public Transform CircleRouletteSpawnPoint;
        public Transform MiningFarmSpawnPoint;
        public Transform DisableClientZone;
        public List<Transform> RandomItemSpawnPoints;
    }
}