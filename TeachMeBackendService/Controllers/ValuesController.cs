using System.Web.Http;
using System.Web.Http.Tracing;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Config;
using TeachMeBackendService.DataObjects;

namespace TeachMeBackendService.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    public class ValuesController : ApiController
    {
        // GET api/values
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
        public string Post()
        {
            return "Hello World!";
        }

        // GET api/values
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
                
                dbContext.Sections.Remove(section);
                dbContext.SaveChanges();
                          
            }

            return Ok();
        }
    }
}
