using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Pages.Admin.Order
{
    public class OrderDetailsModel : PageModel
    {
       
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public OrderDetailsVM OrderDetailsVM { get; set; }
        public void OnGet(long id)
        {
            OrderDetailsVM = new OrderDetailsVM
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(m => m.OrderId == id).ToList()
            };

            OrderDetailsVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(a => a.Id == OrderDetailsVM.OrderHeader.UserId);
        }

        public IActionResult OnPostOrderConfirm(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusCompleted;
            _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderCancel(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusCancelled;
            _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }

        public IActionResult OnPostOrderRefund(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            //Add for Stripe
            //refund amount

            var option = new RefundCreateOptions
            {
                Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                Reason = RefundReasons.RequestedByCustomer,
                //ChargeId=orderHeader.TransactionId
                Charge = orderHeader.TransactionId
            };
            var servic = new RefundService();
            Refund refund = servic.Create(option);

            orderHeader.Status = SD.StatusRefunded;
            _unitOfWork.Save();
            return RedirectToPage("OrderList");
        }
    }
}
