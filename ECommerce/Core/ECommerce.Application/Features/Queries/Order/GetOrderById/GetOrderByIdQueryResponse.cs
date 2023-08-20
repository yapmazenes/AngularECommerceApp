﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public object BasketItems { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public bool Completed { get; set; }
    }
}
