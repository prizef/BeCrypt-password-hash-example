using Newtonsoft.Json.Linq;
using Models.Domain;
using Models.Requests;
using Models.Responses;
using Services;
using Services.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Web.Controllers
{
    public class UsersController : ApiController
    {
        readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [Route("api/users"), HttpPost, AllowAnonymous]
        public HttpResponseMessage Create(UserCreateRequest model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "You did not send any body data!");
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            try
            {
                ItemResponse<int> itemResponse = new ItemResponse<int>();
                itemResponse.Item = usersService.Create(model);
                return Request.CreateResponse(HttpStatusCode.Created, itemResponse);
            }
            catch (DuplicateUserException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email already exists");
            }
        }
    }
}


           

            
        