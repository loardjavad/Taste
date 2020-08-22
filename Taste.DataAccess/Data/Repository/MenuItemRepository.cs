using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Taste.Data;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MenuItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(MenuItem menuItem)
        {
            var menuItemObj = _dbContext.MenuItems.FirstOrDefault(predicate => predicate.Id == menuItem.Id);

            menuItemObj.Name = menuItem.Name;
            menuItemObj.Description = menuItem.Description;
            menuItemObj.Price = menuItem.Price;
            menuItemObj.CategoryId = menuItem.CategoryId;
            menuItemObj.FoodTypeId = menuItem.FoodTypeId;
            menuItemObj.UpdateTime = menuItem.UpdateTime;
            if (menuItem.Image != null)
            {
                menuItemObj.Image = menuItem.Image;
            }

            _dbContext.SaveChanges();
        }
    }
}
