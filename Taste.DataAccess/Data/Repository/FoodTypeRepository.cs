using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Taste.Data;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public FoodTypeRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SelectListItem> GetFoodTypeForDropDown()
        {
            return _dbContext.FoodTypes.Select(i => new SelectListItem() {
                Text=i.Name,
                Value=i.Id.ToString()
            });
        }

        public void RemoveFromList(FoodType foodType)
        {
            var foodtypeObj = _dbContext.FoodTypes.FirstOrDefault(p => p.Id == foodType.Id);
            foodtypeObj.IsRemoved = true;
            foodtypeObj.RemoveTime = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public void Update(FoodType foodType)
        {
            var foodtypeObj = _dbContext.FoodTypes.FirstOrDefault(predicate => predicate.Id == foodType.Id);
            foodtypeObj.Name = foodType.Name;
            foodtypeObj.UpdateTime = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }
}
