using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MockDemo
{
    public class NotImplExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Exception ex = context.Exception;

            Debug.WriteLine(ex.Message); 

        }

        public async override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            await Task.Run(() =>
            {
                Exception ex = actionExecutedContext.Exception;

                Debug.WriteLine(ex.Message);

            });
        }
    }

    [NotImplExceptionFilter]
    public class TestAPIController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            Debug.WriteLine(this.User.Identity.Name);

            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
            Debug.WriteLine(this.User.Identity.Name);

            throw new Exception("xxxxxxxxxxxxxxxxxxxxxxx");
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}