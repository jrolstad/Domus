using System;
using System.Web.Security;
using Domus.Commands;
using Domus.Entities;
using Domus.Providers;
using Domus.Web.UI.Commands.Requests;
using Domus.Web.UI.Commands.Responses;

namespace Domus.Web.UI.Commands
{
    public class AuthenticateUserCommand:ICommand<AuthenticateUserRequest,AuthenticateUserResponse>
    {
        private readonly IRepository<User, string> _userProvider;

        public AuthenticateUserCommand(IRepository<User, string> userProvider)
        {
            _userProvider = userProvider;
        }

        public AuthenticateUserResponse Execute(AuthenticateUserRequest userRequest)
        {
            // Try and get the related user
            var user = _userProvider.Get(userRequest.UserName);

            // If the user is found and password matches, let them in
            if (user != null 
                && string.Equals(user.Password, userRequest.Password, StringComparison.CurrentCultureIgnoreCase))
            {
                FormsAuthentication.SetAuthCookie(userRequest.UserName, true);

                return new AuthenticateUserResponse { IsAuthenticated = true };
            }

            // Otherwise they were not authenticated
            return new AuthenticateUserResponse { IsAuthenticated = false };
        }
    }
}