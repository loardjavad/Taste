using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Utility;

namespace Taste.Pages.Customer.Home
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCard ShoppingCardObj { get; set; }

        public void OnGet(long id)
        {
            ShoppingCardObj = new ShoppingCard()
            {
                MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includeProperties: "Category,FoodType", filter: c => c.Id == id),
                MenuItemId = id
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShoppingCardObj.ApplicationUserId = claim.Value;

                ShoppingCard cartFromDb = _unitOfWork.ShoppingCard.GetFirstOrDefault(c => c.ApplicationUserId == ShoppingCardObj.ApplicationUserId 
                                                                                                    && c.MenuItemId == ShoppingCardObj.MenuItemId);
                if (cartFromDb==null)
                {
                    _unitOfWork.ShoppingCard.Add(ShoppingCardObj);
                }
                else
                {
                    cartFromDb.Count = _unitOfWork.ShoppingCard.IncrementCount(cartFromDb, ShoppingCardObj.Count);
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCard.GetAll(c=>c.ApplicationUserId==ShoppingCardObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCard, count);
                return RedirectToPage("Index");
            }
            else
            {
                ShoppingCardObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includeProperties: "Category,FoodType", filter: c => c.Id == ShoppingCardObj.MenuItemId);
                return Page();
            }
        }
    }
}
