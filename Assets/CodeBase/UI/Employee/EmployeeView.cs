using System;
using Abu;
using CodeBase.Data;
using CodeBase.Services.Employee;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI.Employee
{
    [RequireComponent(typeof(Button))]
    public class EmployeeView : MonoBehaviour
    {
        [field: SerializeField] public TutorialHighlight TutorialHighlight { get; private set; }
        [field: SerializeField] public Button HireButton { get; private set; }
        
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private TMP_Text _qualificationTypeText;
        [SerializeField] private TMP_Text _profitText;
        
        private EmployeeData _employeeData;
        private EmployeeHirerService _employeeHirerService;
        
        public event Action Closed;

        [Inject]
        private void Construct(EmployeeHirerService employeeHirerService) => 
            _employeeHirerService = employeeHirerService;

        private void OnEnable() =>
            HireButton.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            HireButton.onClick.RemoveListener(OnButtonClicked);

        public void SetInfo(EmployeeData employeeData)
        {
            _employeeData = employeeData;
            _nameText.text = employeeData.Name;
            _salaryText.text = $"Salary: {employeeData.Salary}";
            _qualificationTypeText.text = $"Qualification: {employeeData.QualificationType}";
            _profitText.text = $"Profit: {employeeData.Profit}";
        }

        private void OnButtonClicked()
        {
            _employeeHirerService.SetEmployee(_employeeData);
            Closed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}