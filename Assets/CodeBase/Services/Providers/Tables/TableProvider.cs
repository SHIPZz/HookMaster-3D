using System.Collections.Generic;
using CodeBase.Gameplay.TableSystem;
using UnityEngine;

namespace CodeBase.Services.Providers.Tables
{
    public class TableProvider : MonoBehaviour
    {
        [field: SerializeField] public List<Table> Tables;
    }
}