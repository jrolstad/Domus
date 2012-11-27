using System.Web.Security;
using Domus.Commands;
using Domus.Web.UI.Commands.Requests;
using Domus.Web.UI.Commands.Responses;

namespace Domus.Web.UI.Commands
{
    public class SignOutUserCommand:ICommand<SignOutUserRequest,SignOutUserResponse>
    {
        public SignOutUserResponse Execute(SignOutUserRequest request)
        {
            FormsAuthentication.SignOut();

            return new SignOutUserResponse();
        }
    }
}