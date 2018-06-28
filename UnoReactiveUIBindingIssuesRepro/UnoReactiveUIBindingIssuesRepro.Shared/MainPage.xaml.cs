using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using ReactiveUI;
using UnoReactiveUIBindingIssuesRepro.ViewModels;

namespace UnoReactiveUIBindingIssuesRepro
{
    public sealed partial class MainPage : IViewFor<IMainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(
                    ViewModel,
                    x => x.IsRefreshing,
                    x => x.RefreshProgress.Visibility,
                    x => x ? Visibility.Visible : Visibility.Collapsed)
                    .DisposeWith(disposables);
                this.OneWayBind(
                    ViewModel,
                    x => x.IsRefreshing,
                    x => x.ProductsList.Visibility,
                    x => x ? Visibility.Collapsed : Visibility.Visible)
                    .DisposeWith(disposables);
                this.OneWayBind(
                        ViewModel,
                        x => x.Products,
                        x => x.ProductsList.ItemsSource)
                    .DisposeWith(disposables);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (IMainPageViewModel)value;
        }

        public IMainPageViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = e.Parameter as IMainPageViewModel;
        }
    }
}
