using System;
using System.Linq;
using CodeBase.Services.EmployeeHirer;
using CodeBase.Services.Providers.Tables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
        private PotentialEmployeeData _potentialEmployeeData;
        private EmployeeHirerService _employeeHirerService;

        [Inject]
        private void Construct(EmployeeHirerService employeeHirerService)
        {
            _employeeHirerService = employeeHirerService;
        }

        private void OnEnable() =>
            _hireButton.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _hireButton.onClick.RemoveListener(OnButtonClicked);

        public void SetInfo(PotentialEmployeeData potentialEmployeeData)
        {
            _potentialEmployeeData = potentialEmployeeData;
            _nameText.text = potentialEmployeeData.Name;
            _salaryText.text = $"Salary: {potentialEmployeeData.Salary}";
            _qualificationTypeText.text = $"Qualification: {potentialEmployeeData.QualificationType}";
            _profitText.text = $"Profit: {potentialEmployeeData.Profit}";
        }

        private void OnButtonClicked()
        {
            _employeeHirerService.Hire(_potentialEmployeeData);
        }
    }
}