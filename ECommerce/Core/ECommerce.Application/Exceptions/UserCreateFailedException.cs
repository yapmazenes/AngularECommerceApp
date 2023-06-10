using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Exceptions
{
    public class UserCreateFailedException : Exception
    {
        //Todo: Manage all message from Domain
        public UserCreateFailedException() : base("Kullanici olusturulurken beklenmeyen bir hatayla karsilasildi")
        {
        }

        public UserCreateFailedException(string? message) : base(message)
        {
        }

        public UserCreateFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
