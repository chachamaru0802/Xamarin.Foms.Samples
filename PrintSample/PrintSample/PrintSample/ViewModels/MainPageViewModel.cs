using PrintSample.DependencyServices;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly IPrintService _printService;

        public DelegateCommand PrintCommand => new DelegateCommand(async () => await Print());

        public MainPageViewModel(INavigationService navigationService, IDeviceService deviceService, IPageDialogService pageDialogService, IDependencyService dependencyService)
            : base(navigationService, deviceService, pageDialogService, dependencyService)
        {
            Title = "Main Page";

            _printService = _dependencyService.Get<IPrintService>();
        }

        private async Task Print()
        {
            try
            {
                await _printService.PrintAsync("test.pdf");
            }
            catch (Exception e)
            {
                await base.ShowErrorMessage(e);
            }

        }
    }
}
