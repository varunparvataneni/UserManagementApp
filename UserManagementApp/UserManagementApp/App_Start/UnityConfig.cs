using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using UserManagementApp.Service;
using UserManagementApp.Service.Implementation;

namespace UserManagementApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IUserManagement, UserManagement>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}