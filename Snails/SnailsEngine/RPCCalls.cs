using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoBrainsGames.Snails
{
    class RPCCalls
    {
        public enum RPCMethod
        {
            Login,
            CreateNewUser
        }

        public class Login_RemoteCallParams
        {
            public string Username { get; set; }
            public string Password { get; set; }

            public Login_RemoteCallParams(string username, string password)
            {
                this.Username = username;
                this.Password = password;
            }

        }

        public class NewUser_RemoteCallParams
        {
            public string Username { get; set; }
            public string Password { get; set; }

            public NewUser_RemoteCallParams(string username, string password)
            {
                this.Username = username;
                this.Password = password;
            }
        }
    }
}
