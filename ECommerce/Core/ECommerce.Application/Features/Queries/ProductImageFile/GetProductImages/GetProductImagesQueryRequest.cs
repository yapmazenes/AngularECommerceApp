using MediatR;

namespace ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryRequest : IRequest<IEnumerable<GetProductImagesQueryResponse>>
    {
        public string Id { get; set; }
    }
}
