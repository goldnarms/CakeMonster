using Kakemons.UI.Pages.Login;
using Xamarin.Forms;

namespace Kakemons.UI
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            BindingContext = this;

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("login", typeof(LoginPage));
        }
    }
}
