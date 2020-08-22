using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public OrderDetailsCardVM OrderDetailsCardVM { get; set; }
        public void OnGet()
        {
            OrderDetailsCardVM = new OrderDetailsCardVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ListCard=new List<ShoppingCard>()
            };

            OrderDetailsCardVM.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                IEnumerable<ShoppingCard> card = _unitOfWork.ShoppingCard.GetAll(m => m.ApplicationUserId == claim.Value);

                if (card != null)
                {
                    OrderDetailsCardVM.ListCard = card.ToList();
                }

                foreach (var cartList in OrderDetailsCardVM.ListCard)
                {
                    cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == cartList.MenuItemId);
                    OrderDetailsCardVM.OrderHeader.OrderTotal += (cartList.MenuItem.Price * cartList.Count);
                }
            }

        }

        public IActionResult OnPostPlus(long cardId)
        {
            var card = _unitOfWork.ShoppingCard.GetFirstOrDefault(m => m.Id == cardId);
            _unitOfWork.ShoppingCard.IncrementCount(card, 1);
            _unitOfWork.Save();

            return RedirectToPage("/Customer/Cart/Index");
        }
        public IActionResult OnPostMinus(long cardId)
        {
            var card = _unitOfWork.ShoppingCard.GetFirstOrDefault(m => m.Id == cardId);
            if (card.Count == 1)
            {
                _unitOfWork.ShoppingCard.Remove(card);
                _unitOfWork.Save();

                var cnt = _unitOfWork.ShoppingCard.GetAll(m => m.ApplicationUserId == card.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCard, cnt);

            }
            else
            {
                _unitOfWork.ShoppingCard.DecrementCount(card, 1);
                _unitOfWork.Save();
            }


            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(long cardId)
        {
            var card = _unitOfWork.ShoppingCard.GetFirstOrDefault(m => m.Id == cardId);

            _unitOfWork.ShoppingCard.Remove(card);
            _unitOfWork.Save();

            var cnt = _unitOfWork.ShoppingCard.GetAll(m => m.ApplicationUserId == card.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ShoppingCard, cnt);


            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
