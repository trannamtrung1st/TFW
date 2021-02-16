using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TFW.Cross.Models;

namespace TFW.WebAPI.Controllers
{
    public abstract class BaseApiController : ControllerBase
    {
        protected readonly DbContext dbContext;

        public PrincipalInfo UserInfo
        {
            get
            {
                object principalInfo;
                var items = HttpContext.Items;

                if (items.TryGetValue(ControllerConsts.PrincipalInfoItemKey, out principalInfo))
                    return principalInfo as PrincipalInfo;

                return new PrincipalInfo();
            }
        }

        protected T Service<T>()
        {
            return HttpContext.RequestServices.GetRequiredService<T>();
        }

        protected IActionResult Error(object obj = default)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, obj);
        }

        protected string GetAuthorityLeftPart()
        {
            return new Uri(Request.GetEncodedUrl()).GetLeftPart(UriPartial.Authority);
        }
    }
}
