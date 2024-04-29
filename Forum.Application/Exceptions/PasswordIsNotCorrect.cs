using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forum.Application.Exceptions
{
    public class PasswordIsNotCorrect : Exception
    {
        public PasswordIsNotCorrect(string? message) : base("Password is incorrect "+message)
        {
        }
    }
}
