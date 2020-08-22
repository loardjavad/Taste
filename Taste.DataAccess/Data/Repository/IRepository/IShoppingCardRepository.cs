using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface IShoppingCardRepository : IRepository<ShoppingCard>
    {
        int IncrementCount(ShoppingCard shoppingCard,int count);
        int DecrementCount(ShoppingCard shoppingCard,int count);
    }
}
