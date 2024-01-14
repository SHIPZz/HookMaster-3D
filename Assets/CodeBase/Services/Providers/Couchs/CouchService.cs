using System.Collections.Generic;
using System.Linq;
using CodeBase.Gameplay.CouchSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.Services.Providers.Couchs
{
    public class CouchService : SerializedMonoBehaviour
    {
        public List<Couch> Couches = new();

        public Transform GetSitPlace()
        {
            Transform target = null;
            
            return Couches.Any(x => x.HasFreeSide(out target)) ? target : null;
        }
    }
}