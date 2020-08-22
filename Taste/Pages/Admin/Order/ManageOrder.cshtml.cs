using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ManageOrderModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManageOrderModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public List<OrderDetailsVM> orderDetailsVMs { get; set; }

        public void OnGet()
        {
            orderDetailsVMs = new List<OrderDetailsVM>();

            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader
                .GetAll(o => o.Status == SD.StatusSubmitted || o.Status == SD.StatusInProcess)
                .OrderByDescending(p => p.PickUpTime).ToList();

            foreach (OrderHeader item in orderHeaders)
            {
                OrderDetailsVM orderDetailsVM = new OrderDetailsVM
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(p => p.OrderId == item.Id).ToList()
                };
                orderDetailsVMs.Add(orderDetailsVM);
            }
        }

        public IActionResult OnPostOrderPrepare(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusInProcess;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderReady(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusReady;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderCancel(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            orderHeader.Status = SD.StatusCancelled;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }

        public IActionResult OnPostOrderRefund(long orderId)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == orderId);
            //Add for Stripe
            //refund amount

            var option = new RefundCreateOptions
            {
                Amount=Convert.ToInt32(orderHeader.OrderTotal*100),
                Reason=RefundReasons.RequestedByCustomer,
                //ChargeId=orderHeader.TransactionId
                Charge=orderHeader.TransactionId
            };
            var servic = new RefundService();
            Refund refund = servic.Create(option);

            orderHeader.Status = SD.StatusRefunded;
            _unitOfWork.Save();
            return RedirectToPage("ManageOrder");
        }
    }
}
