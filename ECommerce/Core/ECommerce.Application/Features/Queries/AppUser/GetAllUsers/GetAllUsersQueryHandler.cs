using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Features.Queries.AppUser.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public GetAllUsersQueryHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<GetAllUsersQueryResponse> Handle(GetAllUsersQueryRequest request, CancellationToken cancellationToken)
        {
            var usersCount = await _userManager.Users.CountAsync();
            var users = await _userManager.Users.Skip(request.Page * request.Size).Take(request.Size)
                .Select(user => new
                {
                    Id = user.Id,
                    Email = user.Email,
                    NameSurname = user.NameSurname,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    UserName = user.UserName
                }).ToListAsync();

            return new() { Datas = users, TotalCount = usersCount };
        }

    }
}
