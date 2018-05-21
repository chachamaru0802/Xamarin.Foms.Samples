using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Samples.DependencyServices;

namespace Xamarin.Forms.Samples.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        readonly IPrintService _printService;


        public DelegateCommand PrintCommand => new DelegateCommand(async () => await Print());



        public MainPageViewModel(IDependencyService dependencyService) : base(dependencyService)
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
