using ECommerce.Application.RepositoryAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage
{
    public class ChangeShowcaseImageCommandHandler : IRequestHandler<ChangeShowcaseImageCommandRequest, ChangeShowcaseImageCommandResponse>
    {
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public ChangeShowcaseImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        public async Task<ChangeShowcaseImageCommandResponse> Handle(ChangeShowcaseImageCommandRequest request, CancellationToken cancellationToken)
        {
            var query = _productImageFileWriteRepository.Table
                .Include(p => p.Products)
                .SelectMany(p => p.Products, (pif, p) => new
                {
                    pif,
                    p
                });

            var data = await query.FirstOrDefaultAsync(x => x.p.Id == request.ProductId && x.pif.Showcase == true);

            if (data != null)
            {
                data.pif.Showcase = false;
            }

            var image = await query.FirstOrDefaultAsync(x => x.pif.Id == request.ImageId);

            if (image != null)
            {
                image.pif.Showcase = true;
            }

            await _productImageFileWriteRepository.SaveAsync();

            return new();
        }
    }
}
