using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Taste.Data;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return _dbContext.Categories.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Category category)
        {
            var objFromDb = _dbContext.Categories.FirstOrDefault(s => s.Id == category.Id);
            objFromDb.Name = category.Name;
            objFromDb.DisplayOrder = category.DisplayOrder;
            objFromDb.UpdateTime = DateTime.Now;

            _dbContext.SaveChanges();
        }
        public void RemoveFromList(Category category)
        {
            var objFromDb = _dbContext.Categories.FirstOrDefault(s => s.Id == category.Id);
            objFromDb.IsRemoved = true;
            objFromDb.RemoveTime = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}
