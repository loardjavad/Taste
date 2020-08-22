using System;
using System.Collections.Generic;
using System.Text;
using Taste.Data;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Category = new CategoryRepository(_context);
            FoodType = new FoodTypeRepository(_context);
            MenuItem = new MenuItemRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            ShoppingCard = new ShoppingCardRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context);
        }

        public ICategoryRepository Category { get; private set; }

        public IFoodTypeRepository FoodType { get; private set; }
        public IMenuItemRepository MenuItem { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IShoppingCardRepository ShoppingCard { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
