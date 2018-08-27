using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using TeachMeBackendService.Models;

namespace TeachMeBackendService.ControllersTables
{
    public abstract class BaseController<T> : TableController<T> where T : class, ITableData
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var context = new TeachMeBackendContext();
            SetDomainManager(new EntityDomainManager<T>(context, Request));
        }

        public void SetDomainManager(EntityDomainManager<T> domainManager)
        {
            DomainManager = domainManager;
        }
    }
}