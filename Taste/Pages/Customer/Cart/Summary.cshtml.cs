using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public OrderDetailsCardVM OrderDetailsCardVM { get; set; }
        public IActionResult OnGet()
        {
            OrderDetailsCardVM = new OrderDetailsCardVM()
            {
                OrderHeader = new Models.OrderHeader()
            };

            OrderDetailsCardVM.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

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

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(m => m.Id == claim.Value);
            OrderDetailsCardVM.OrderHeader.PickUpName = applicationUser.FullName;
            OrderDetailsCardVM.OrderHeader.PickUpTime = DateTime.Now;
            OrderDetailsCardVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            return Page();
        }

        public IActionResult OnPost(string stripToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsCardVM.ListCard = _unitOfWork.ShoppingCard.GetAll(m => m.ApplicationUserId == claim.Value).ToList();

            OrderDetailsCardVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            OrderDetailsCardVM.OrderHeader.UserId = claim.Value;
            OrderDetailsCardVM.OrderHeader.Status = SD.PaymentStatusPending;
            OrderDetailsCardVM.OrderHeader.PickUpTime = Convert.ToDateTime(OrderDetailsCardVM.OrderHeader.PickUpDate.ToShortDateString() + " " + OrderDetailsCardVM.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _unitOfWork.OrderHeader.Add(OrderDetailsCardVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in OrderDetailsCardVM.ListCard)
            {
                item.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(p => p.Id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = OrderDetailsCardVM.OrderHeader.Id,
                    Description = item.MenuItem.Description,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };

                OrderDetailsCardVM.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price);
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            OrderDetailsCardVM.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderDetailsCardVM.OrderHeader.OrderTotal));
            _unitOfWork.ShoppingCard.RemoveRange(OrderDetailsCardVM.ListCard);
            HttpContext.Session.SetInt32(SD.ShoppingCard, 0);
            _unitOfWork.Save();

            //Add for Stripe
            if (stripToken != null)
            {
                var option = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderDetailsCardVM.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID: " + OrderDetailsCardVM.OrderHeader.Id,
                    Source = stripToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(option);

                OrderDetailsCardVM.OrderHeader.TransactionId = charge.Id;

                if (charge.Status.ToLower() == "succeeded")
                {
                    OrderDetailsCardVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    OrderDetailsCardVM.OrderHeader.Status = SD.StatusSubmitted;
                }
                else
                {
                    OrderDetailsCardVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;

                }
            }
            else
            {
                OrderDetailsCardVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;

            }

            _unitOfWork.Save();

            return RedirectToPage("/Customer/Cart/OrderConfirmation", new { id = OrderDetailsCardVM.OrderHeader.Id });
        }
    }
}
