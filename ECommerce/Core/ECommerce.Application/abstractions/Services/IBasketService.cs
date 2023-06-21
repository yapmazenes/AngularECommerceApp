using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Abstractions.Services
{
    public interface IBasketService
    {
        public Task<IEnumerable<BasketItem>> GetBasketItemsAsync();
        public Task AddItemToBasketAsync(Guid productId, int quantity);
        public Task UpdateQuantityAsync(string basketItemId, int quantity);
        public Task RemoveBasketItemAsync(string basketItemId);
    }
}
