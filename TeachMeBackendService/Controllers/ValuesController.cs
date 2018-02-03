using System.Web.Http;
using System.Web.Http.Tracing;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using TeachMeBackendService.DataObjects;
using Microsoft.Web.Http;

namespace TeachMeBackendService.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:ApiVersion}/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route("")]
        public string Get()
        {
            MobileAppSettingsDictionary settings = this.Configuration.GetMobileAppSettingsProvider().GetMobileAppSettings();
            ITraceWriter traceWriter = this.Configuration.Services.GetTraceWriter();

            string host = settings.HostName ?? "localhost";
            string greeting = "Hello from " + host;
            
            traceWriter.Info(greeting);
            return greeting;
        }

        // POST api/values
        [Route("")]
        public string Post()
        {
            return "Hello World!";
        }

        // GET api/values
        [Route("{id}")]
        public IHttpActionResult GetValueById(string id)
        {
            Section section = null;

            using (var ctx = new Models.TeachMeBackendContext())
            {
                section = ctx.Sections.Find(id);
            }

            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
        }


        // Delete api/values
        [Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            using (var dbContext = new Models.TeachMeBackendContext())
            {
                //var section = dbContext.Sections.Find(id);
                var section = dbContext.Sections.Include(c => c.Lessons).FirstOrDefault(c => c.Id == id);

                if (section == null)
                {
                    return NotFound();
                }

                dbContext.Lessons.RemoveRange(section.Lessons);
                dbContext.Sections.Remove(section);
                dbContext.SaveChanges();
                          
            }

            return Ok();
        }
    }
}
