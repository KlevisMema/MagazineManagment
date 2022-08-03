using System.Web.Http;
using System.Web.Http.Controllers;

namespace MagazineManagment.Shared.CustomAuthorizationFilter
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}
