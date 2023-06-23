using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Exceptions
{
    public class NotFoundBasketException : Exception
    {
        public NotFoundBasketException () : base("Username or password wrong")
        {
        }

        public NotFoundBasketException (string? message) : base(message)
        {
        }

        public NotFoundBasketException (string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
