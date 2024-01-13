using System.Collections.Generic;
using CodeBase.Constant;
using CodeBase.Data;
using CodeBase.SO.Materials;
using UnityEngine;

namespace CodeBase.Services.DataService
{
    public class MaterialStaticDataService
    {
        private readonly IReadOnlyDictionary<MaterialTypeId, Material> _materials;

        public MaterialStaticDataService()
        {
            _materials = Resources.Load<MaterialSO>(AssetPath.MaterialSO).Materials;
        }

        public Material GetMaterial(MaterialTypeId materialTypeId) =>
            _materials[materialTypeId];
    }
}