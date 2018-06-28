using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace UnoReactiveUIBindingIssuesRepro.Shared
{
    public abstract partial class BasePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = e.Parameter;
            base.OnNavigatedTo(e);
        }
    }
}
