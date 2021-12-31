using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
    public class RegisterUserRequest : IRequest
    {
        public string Username { get; set; }

        public RegisterUserRequest(string username)
		{
            this.Username = username;
        }
    }
}
