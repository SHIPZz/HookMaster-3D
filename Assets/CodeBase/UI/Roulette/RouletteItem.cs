using System;
using System.Globalization;
using CodeBase.Data;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.UI.Roulette
{
    public class RouletteItem : MonoBehaviour
    {
        [field: SerializeField] public int Quantity { get; private set; }
        [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
        
        [SerializeField] private TMP_Text _quantityText;
        
        public void Init(int minValue, int maxValue)
        {
            Quantity = Random.Range(minValue, maxValue);
            _quantityText.text = Quantity.ToString(CultureInfo.InvariantCulture);
        }
    }
}