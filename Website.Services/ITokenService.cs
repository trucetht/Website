using System;
using System.Collections.Generic;
using System.Text;
using Website.Models.Account;

namespace Website.Services
{
    public interface ITokenService
    {
        public string CreateToken(ApplicationUserIdentity user);
    }
}
