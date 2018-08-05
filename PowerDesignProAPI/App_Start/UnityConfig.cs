using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Mapper.FromDto;
using PowerDesignPro.BusinessProcessors.Processors;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data;
using PowerDesignPro.Data.Framework.DbFactory;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Framework.Repository;
using System;
using System.Web;
using PowerDesignPro.BusinessProcessors.Mapper.ToDto;
using PowerDesignPro.Data.Models;

namespace PowerDesignProAPI
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // TODO: Register your types here
            //container.RegisterType<IdentityDbContext<ApplicationUser>, ApplicationDbContext>(new PerThreadLifetimeManager());
            container.RegisterType<IDbFactory, DbFactory>(new HierarchicalLifetimeManager());
            //container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            //container.RegisterType<ApplicationSignInManager>();
            //container.RegisterType<ApplicationUserManager>();
            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new InjectionConstructor(typeof(ApplicationDbContext)));
            //container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));
            container.RegisterType(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));

            //Registering the processors
            container.RegisterType<IUserDashboard, UserDashboardProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IProject, ProjectProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IPickList, PickListProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectSolution, ProjectSolutionProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<ITraceMessage, TraceMessageProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<ISolutionLoad, SolutionLoadProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<ISolutionSummary, SolutionSummaryProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IGeneratorAlternator, GeneratorAlternatorProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IAdmin, AdminProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IAdminLoad, AdminLoadProcessor>(new HierarchicalLifetimeManager());
            container.RegisterType<IPDF, PDFProcessor>(new HierarchicalLifetimeManager());

            //Registering Mappers
            container.RegisterType(typeof(IMapper<,>), typeof(AutoMapper<,>));
            container.RegisterType(typeof(IMapper<AddProjectDto, Project>), typeof(AddProjectDtoToProjectEntityMapper));
            container.RegisterType(typeof(IMapper<Project, ProjectDetailDto>), typeof(ProjectEntityToProjectDetailDtoMapper));
            container.RegisterType(typeof(IMapper<ProjectSolutionDto, Solution>), typeof(SolutionRequestDtoToSolutionEntityMapper));
            container.RegisterType(typeof(IMapper<Solution, SolutionHeaderDetailsInProjectDto>), typeof(SolutionEntityToSolutionHeaderDetailsDtoMapper));
        }
    }
}