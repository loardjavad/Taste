using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Taste.Data;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
   public class ShoppingCardRepository : Repository<ShoppingCard>, IShoppingCardRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ShoppingCardRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public int DecrementCount(ShoppingCard shoppingCard, int count)
        {
            shoppingCard.Count -= count;
            return shoppingCard.Count;
        }

        public int IncrementCount(ShoppingCard shoppingCard, int count)
        {
            shoppingCard.Count += count;
            return shoppingCard.Count;
        }
    }
}
