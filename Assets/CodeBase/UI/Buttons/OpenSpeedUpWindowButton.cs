using System.Linq;
using CodeBase.Data;
using CodeBase.Services.WorldData;
using CodeBase.UI.SpeedUp;
using CodeBase.UI.UpgradeEmployee;
using Zenject;

namespace CodeBase.UI.Buttons
{
    public class OpenSpeedUpWindowButton : ButtonOpenerBase
    {
        private EmployeeData _employeeData;
        private IWorldDataService _worldDataService;

        [Inject]
        private void Construct(IWorldDataService worldDataService)
        {
            _worldDataService = worldDataService;
        }
        
        public void SetEmployeeData(EmployeeData employeeData) =>
            _employeeData = employeeData;
        
        protected override void Open()
        {
            WindowService.Close<UpgradeEmployeeWindow>();
            var speedUpWindow = WindowService.Get<SpeedUpWindow>();

            UpgradeEmployeeData targetUpgradeEmployeeData = _worldDataService.WorldData
                .UpgradeEmployeeDatas
                .FirstOrDefault(x => x.EmployeeData.Guid == _employeeData.Guid) ??
                                                            new UpgradeEmployeeData { EmployeeData = _employeeData};
            print(targetUpgradeEmployeeData);
            _worldDataService.Save();
            speedUpWindow.SetInfo(targetUpgradeEmployeeData);
            speedUpWindow.Open();
        }
    }
}