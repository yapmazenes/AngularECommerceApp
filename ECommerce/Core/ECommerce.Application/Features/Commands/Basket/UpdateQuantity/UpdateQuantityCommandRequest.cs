﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.Basket.UpdateQuantity
{
    public class UpdateQuantityCommandRequest : IRequest<UpdateQuantityCommandResponse>
    {
        public Guid BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
