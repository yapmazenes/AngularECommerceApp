﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandRequest : IRequest<CompleteOrderCommandResponse>
    {
        public Guid OrderId { get; set; }
    }
}
