using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CodeBase.MaterialChanger
{
    public class ChildMeshMaterialChanger : MeshMaterialChanger
    {
        [SerializeField] private List<MeshMaterialChanger> _meshMaterialChangers;

        [Button]
        public override void SetInitialMaterial()
        {
            _meshMaterialChangers.ForEach(x =>x.SetInitialMaterial());
            base.SetInitialMaterial();
        }

        [Button]
        public override void Change()
        {
            _meshMaterialChangers.ForEach(x => x.Change());
            base.Change();
        }
    }
}