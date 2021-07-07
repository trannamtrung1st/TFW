using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TAuth.ResourceClient.Auth.Policies
{
    public class DeleteResourceRequirement : IAuthorizationRequirement
    {
        public string TestName { get; set; }
    }

    public class ResourceAuthorizationModel
    {
        public string Name { get; set; }
        public int OwnerId { get; set; }
    }

    public class DeleteTestResourceHandler : AuthorizationHandler<DeleteResourceRequirement, ResourceAuthorizationModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteResourceRequirement requirement, ResourceAuthorizationModel resource)
        {
            if (resource.Name == requirement.TestName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class DeleteOwnedResourceHandler : AuthorizationHandler<DeleteResourceRequirement, ResourceAuthorizationModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteResourceRequirement requirement, ResourceAuthorizationModel resource)
        {
            var userId = context.User.FindFirst(JwtClaimTypes.Subject).Value;

            if (userId == $"{resource.OwnerId}")
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

    public class AdminDeleteResourceHandler : AuthorizationHandler<DeleteResourceRequirement, ResourceAuthorizationModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteResourceRequirement requirement, ResourceAuthorizationModel resource)
        {
            if (context.User.IsInRole("Administrator"))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
