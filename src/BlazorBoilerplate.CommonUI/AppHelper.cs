using BlazorBoilerplate.Components.Mat.AccountManagement.Views;
using BlazorBoilerplate.Components.Mat.Admins.Views;
using BlazorBoilerplate.Components.Mat.Dashboards;
using BlazorBoilerplate.Components.Mat.ExternalAuth.Views;
using System.Reflection;

namespace BlazorBoilerplate.CommonUI
{
    public static class AppHelper
    {
        public static Assembly[] GetRouteComponentsAssemblies()
        {
            return
                new[]
                {
                    typeof(UsersComponent).Assembly,
                    typeof(Dashboard).Assembly,
                    typeof(Register).Assembly,
                    typeof(Confirm).Assembly
                };
        }
    }
}
