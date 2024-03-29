﻿using ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Token
{
    public interface ITokenHandler
    {
        DTOs.Token CreateAccessToken(AppUser appUser);
        DTOs.Token CreateAccessToken(int expirationMinute, AppUser appUser);
        string CreateRefreshToken();
    }
}
