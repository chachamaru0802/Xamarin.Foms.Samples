using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PrintSample.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        /// <summary>
        /// DependencyService
        /// DI取得用
        /// </summary>
        protected readonly IDependencyService _dependencyService;

        /// <summary>
        /// NavigationService
        /// </summary>
        readonly INavigationService _navigationService;

        /// <summary>
        /// デバイス情報サービス
        /// </summary>
        readonly IDeviceService _deviceService;

        /// <summary>
        /// メッセージダイアログサービス
        /// </summary>
        readonly IPageDialogService _pageDialogService;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase(INavigationService navigationService, IDeviceService deviceService, IPageDialogService pageDialogService , IDependencyService dependencyService)
        {
            _navigationService = navigationService;
            _deviceService = deviceService;
            _pageDialogService = pageDialogService;
            _dependencyService = dependencyService;
        }

        internal async Task ShowMessage(string title, string message)
        {
            await _pageDialogService.DisplayAlertAsync(title, message, "OK");
        }

        internal async Task ShowErrorMessage(Exception e)
        {
            await _pageDialogService.DisplayAlertAsync("Error", e.Message, "OK");
        }

        #region Navigation関連

        public virtual void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        #endregion

        public virtual void Destroy()
        {

        }
    }
}
