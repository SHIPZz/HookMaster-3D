using System.Collections.Generic;
using CodeBase.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CodeBase.SO.Materials
{
    [CreateAssetMenu(fileName = nameof(MaterialSO), menuName = "Gameplay/Materials/MaterialSO")]
    public class MaterialSO : SerializedScriptableObject
    {
        [OdinSerialize] private Dictionary<MaterialTypeId, Material> _materials;

        public IReadOnlyDictionary<MaterialTypeId, Material> Materials => _materials;
    }
}