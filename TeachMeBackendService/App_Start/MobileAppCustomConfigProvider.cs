using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System;
using System.Web.Http.Controllers;

namespace TeachMeBackendService
{
    sealed class MobileAppCustomConfigProvider : MobileAppControllerConfigProvider
    {
        readonly Lazy<JsonSerializerSettings> _settings = new Lazy<JsonSerializerSettings>(JsonConvert.DefaultSettings);

        public JsonSerializerSettings Settings => _settings.Value;

        public override void Configure(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            base.Configure(controllerSettings, controllerDescriptor);

            //to get all controllers to respect JsonConvert.DefaultSettings
            controllerSettings.Formatters.JsonFormatter.SerializerSettings = Settings;
        }
    }
}