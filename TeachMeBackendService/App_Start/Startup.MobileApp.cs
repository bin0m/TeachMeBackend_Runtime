﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using TeachMeBackendService.DataObjects;
using TeachMeBackendService.Models;
using Owin;
using System.Web.Http.Tracing;
using System.Web.Http.Routing;
using Microsoft.Web.Http.Routing;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace TeachMeBackendService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {   
            //don't serialize null properties in response json
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            var mobileAppCustomConfigProvider = new MobileAppCustomConfigProvider();

            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();
            SystemDiagnosticsTraceWriter traceWriter = config.EnableSystemDiagnosticsTracing();
            traceWriter.IsVerbose = true;
            traceWriter.MinimumLevel = TraceLevel.Debug;

            // Enable API Versioning
            config.AddApiVersioning();            
           
            var constraintResolver = new DefaultInlineConstraintResolver()
                {
                    ConstraintMap =
                    {
                        ["apiVersion"] = typeof( ApiVersionRouteConstraint )
                    }
                };
            // Enable attribute routing
            config.MapHttpAttributeRoutes(constraintResolver);

            // Convention-based routing (Not sure, it is needed)
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v{version:ApiVersion}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            config.Formatters.JsonFormatter.SerializerSettings = mobileAppCustomConfigProvider.Settings;


            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .WithMobileAppControllerConfigProvider(mobileAppCustomConfigProvider)
                .ApplyTo(config);

            // Include in response full exception detail
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;


            // Создаем базу каждый раз с тестовым сетом данных
            Database.SetInitializer(new TeachMeBackendInitializer());   


            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<TeachMeBackendContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            ConfigureAuth(app);
            CreateRoles();

            app.UseWebApi(config);
        }

        // In this method we will create default User roles
        private static void CreateRoles()
        {
            TeachMeBackendContext context = new TeachMeBackendContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            
            if (!roleManager.RoleExists(UserRole.Student.ToString()))
            {
                roleManager.Create(new IdentityRole { Name = UserRole.Student.ToString()});
            }

            if (!roleManager.RoleExists(UserRole.Teacher.ToString()))
            {
                roleManager.Create(new IdentityRole { Name = UserRole.Teacher.ToString() });
            }

            if (!roleManager.RoleExists(UserRole.Admin.ToString()))
            {
                roleManager.Create(new IdentityRole { Name = UserRole.Admin.ToString() });
            }
        }
    }

    // DropCreateDatabaseIfModelChanges Указывает EF, что если модель изменилась,
    // нужно воссоздать базу данных с новой структурой
    //Alternatives: CreateDatabaseIfNotExists, DropCreateDatabaseIfModelChanges, DropCreateDatabaseAlways 
    public class TeachMeBackendInitializer : CreateDatabaseIfNotExists<TeachMeBackendContext>
    {
        protected override void Seed(TeachMeBackendContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }
                       
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            try
            {
                //1. Add Admin user
                var appUser = new ApplicationUser()
                {
                    UserName = "levitg@gmail.com",
                    Email = "levitg@gmail.com",
                    FullName = "Administrator"
                };

                userManager.Create(appUser, "qweqwe123");

                userManager.AddToRole(appUser.Id, "Admin");

                var user = new User
                {
                    Id = appUser.Id,
                    CompletedCoursesCount = 0,
                    Email = appUser.Email,
                    FullName = appUser.FullName,
                    Login = "admin",
                    RegisterDate = DateTime.Now,
                    UserRole = UserRole.Admin,
                    DateOfBirth = new DateTime(1985, 03, 15)
                };

                context.Set<User>().Add(user);

                //2. Add Simple user
                var appUser2 = new ApplicationUser()
                {
                    UserName = "levitgspam@gmail.com",
                    Email = "levitgspam@gmail.com",
                    FullName = "Иван Простов"
                };

                userManager.Create(appUser2, "qweqwe123");

                userManager.AddToRole(appUser2.Id, "Student");

                var user2 = new User
                {
                    Id = appUser2.Id,
                    CompletedCoursesCount = 0,
                    Email = appUser2.Email,
                    FullName = appUser2.FullName,
                    Login = "admin",
                    RegisterDate = DateTime.Now,
                    UserRole = UserRole.Student,
                    DateOfBirth = new DateTime(1985, 03, 15)
                };

                context.Set<User>().Add(user2);
                context.SaveChanges();
                base.Seed(context);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

