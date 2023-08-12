﻿using ECommerce.Application.RepositoryAbstractions;
using ECommerce.Domain.Entities;
using ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class MenuReadRepository : ReadRepository<Menu>, IMenuReadRepository
    {
        public MenuReadRepository(ECommerceDbContext context) : base(context)
        {
        }
    }
}
