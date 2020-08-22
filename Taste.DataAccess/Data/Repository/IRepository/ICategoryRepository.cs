using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<SelectListItem> GetCategoryListForDropDown();

        void Update(Category category);
        void RemoveFromList(Category category);
    }
}
