using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace TeachMeBackendService
{
    sealed class MobileAppCustomConfigProvider : MobileAppControllerConfigProvider
    {
        readonly Lazy<JsonSerializerSettings> settings = new Lazy<JsonSerializerSettings>(JsonConvert.DefaultSettings);

        public JsonSerializerSettings Settings => settings.Value;

        public override void Configure(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            base.Configure(controllerSettings, controllerDescriptor);

            //to get all controllers to respect JsonConvert.DefaultSettings
            controllerSettings.Formatters.JsonFormatter.SerializerSettings = Settings;
        }
    }
}