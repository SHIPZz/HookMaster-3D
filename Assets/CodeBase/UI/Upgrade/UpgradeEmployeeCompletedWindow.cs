using CodeBase.Animations;
using CodeBase.Data;
using CodeBase.Services.Providers.EmployeeProvider;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Upgrade
{
    public class UpgradeEmployeeCompletedWindow : WindowBase
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private CanvasAnimator _canvasAnimator;
        [Header("Old data")] [SerializeField] private TMP_Text _oldProfitText;
        [SerializeField] private TMP_Text _oldSalaryText;
        [SerializeField] private TMP_Text _oldQualificationTypeText;

        [Header("Upgraded data")]
        [SerializeField] private TMP_Text _newProfitText;
        [SerializeField] private TMP_Text _newSalaryText;
        [SerializeField] private TMP_Text _newQualificationTypeText;

        private EmployeeData _employeeData;

        private EmployeeService _employeeService;

        [Inject]
        private void Construct(EmployeeService employeeService) => 
            _employeeService = employeeService;

        public override void Open()
        {
            _canvasAnimator.FadeInCanvas();
            
            _employeeService.Upgrade(_employeeData, newEmployeeData =>
            {
                _newProfitText.text = $"{newEmployeeData.Profit}$";
                _newSalaryText.text = $"{newEmployeeData.Salary}$";
                _newQualificationTypeText.text = $"{newEmployeeData.QualificationType}";
            });
        }

        public override void Close()
        {
            _canvasAnimator.FadeOutCanvas(base.Close);
        }

        public void Init(EmployeeData employeeData)
        {
            _employeeData = employeeData;
            _oldProfitText.text = $"{_employeeData.Profit}$";
            _oldSalaryText.text = $"{_employeeData.Salary}$";
            _oldQualificationTypeText.text = $"{_employeeData.QualificationType}";
            _nameText.text = $"{_employeeData.Name}";
        }
    }
}