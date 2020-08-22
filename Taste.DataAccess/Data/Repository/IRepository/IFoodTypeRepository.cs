using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface IFoodTypeRepository : IRepository<FoodType>
    {
        IEnumerable<SelectListItem> GetFoodTypeForDropDown();

        void Update(FoodType foodType);
        void RemoveFromList(FoodType foodType);
    }
}
