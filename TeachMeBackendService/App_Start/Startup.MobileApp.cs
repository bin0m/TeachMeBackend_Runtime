using System;
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
using System.Net.Http.Formatting;

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


            app.UseWebApi(config);
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

            List<User> testUsersSet = new List<User> {
                new User {
                    Id = Guid.NewGuid().ToString(),
                    CompletedCoursesCount = 0,
                    Email = "nikolaev12@mail.ru",
                    FullName = "Сергей Николаев",
                    Login = "nikolaev",
                    Password = "aFA4baf34B9e83b3876e=",
                    RegisterDate = new DateTime(2017, 12, 1),
                    },
                new User {
                    Id = Guid.NewGuid().ToString(),
                    CompletedCoursesCount = 0,
                    Email = "pugaeva.verchik@yandex.ru",
                    FullName = "Вероника Пугаева",
                    Login = "verchik",
                    Password = "4C0brainik6EF0B",
                    RegisterDate = new DateTime(2017, 12, 2),
                    }
            };

            foreach (User user in testUsersSet)
            {
                context.Set<User>().Add(user);
            }

            context.SaveChanges();
            base.Seed(context);
        }
    }
}

