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
        [field: SerializeField] public RouletteItemTypeId RouletteItemTypeId { get; private set; }
        
        [SerializeField] private TMP_Text _quantityText;
        [SerializeField] private int _minRandomValue;
        [SerializeField] private int _maxRandomValue;
        
        public void Init()
        {
            Quantity = Random.Range(_minRandomValue, _maxRandomValue);
            _quantityText.text = Quantity.ToString(CultureInfo.InvariantCulture);
        }
    }
}