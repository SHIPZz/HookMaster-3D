using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    [RequireComponent(typeof(Button))]
    public class EmployeeView : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private TMP_Text _qualificationTypeText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private Button _hireButton;

        public event Action<String> Selected;

        private void OnEnable() => 
            _hireButton.onClick.AddListener(OnButtonClicked);

        private void OnDisable() => 
            _hireButton.onClick.RemoveListener(OnButtonClicked);

        public void SetInfo(string name, int salary, int qualificationType, int profit)
        {
            _nameText.text = name;
            _salaryText.text = $"Salary: {salary}";
            _qualificationTypeText.text = $"Qualification: {qualificationType}";
            _profitText.text = $"Profit: {profit}";
        }

        private void OnButtonClicked() => 
            Selected?.Invoke(_name);
    }
}