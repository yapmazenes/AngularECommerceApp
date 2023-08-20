using MediatR;

namespace ECommerce.Application.Features.Commands.Product.UpdateProductStockWithQRCode
{
    public class UpdateProductStockWithQRCodeCommandRequest : IRequest<UpdateProductStockWithQRCodeCommandResponse>
    {
        public Guid ProductId { get; set; }
        
        public int Stock { get; set; }
    }
}
