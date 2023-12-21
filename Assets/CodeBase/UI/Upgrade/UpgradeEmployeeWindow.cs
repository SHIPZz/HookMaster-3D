using CodeBase.Extensions;
using CodeBase.UI.Buttons;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.UpgradeEmployee
{
    public class UpgradeEmployeeWindow : WindowBase
    {
        [SerializeField] private TMP_Text _qualificationTypeText;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _profitText;
        [SerializeField] private TMP_Text _salaryText;
        [SerializeField] private OpenSpeedUpWindowButton _openSpeedUpWindowButton;
        [SerializeField] private CanvasAnimator _canvasAnimator;

        private Gameplay.EmployeeSystem.Employee _employee;

        public override void Open()
        {
            gameObject.SetActive(true);
            _canvasAnimator.FadeInCanvas();
        }

        public override void Close() => 
            _canvasAnimator.FadeOutCanvas(base.Close);

        public void Init(Gameplay.EmployeeSystem.Employee employee)
        {
            _employee = employee;
            _openSpeedUpWindowButton.SetEmployeeData(_employee.ToEmployeeData());
            _profitText.text = $"Profit: {_employee.Profit}";
            _qualificationTypeText.text = $"Qualification type: {_employee.QualificationType}";
            _nameText.text = $"Name: {_employee.Name}";
            _salaryText.text = $"Salary: {_employee.Salary}";
        }
    }
}