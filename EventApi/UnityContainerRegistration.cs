namespace EventApi
{
    using System.Web.Http;

    using Event.Repository;
    using Event.Repository.Interface;

    using Unity;
    using Unity.AspNet.WebApi;

    public class UnityContainerRegistration
    {
        public static IUnityContainer InitialiseContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ICompanyRepository, CompanyRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            return container;
        }
    }
}