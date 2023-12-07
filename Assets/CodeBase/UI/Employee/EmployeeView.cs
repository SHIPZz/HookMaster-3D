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
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private TMP_Text _qualificationTypeText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private Button _hireButton;
        private EmployeeData _employeeData;
        private EmployeeHirerService _employeeHirerService;

        [Inject]
        private void Construct(EmployeeHirerService employeeHirerService) => 
            _employeeHirerService = employeeHirerService;

        private void OnEnable() =>
            _hireButton.onClick.AddListener(OnButtonClicked);

        private void OnDisable() =>
            _hireButton.onClick.RemoveListener(OnButtonClicked);

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
            gameObject.SetActive(false);
        }
    }
}