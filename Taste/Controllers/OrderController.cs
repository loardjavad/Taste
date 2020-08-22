using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(string status = null)
        {
            List<OrderDetailsVM> orderDetailsVMs = new List<OrderDetailsVM>();
            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.CustomerRole))
            {
                //retrieve all order for that customer
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(m => m.UserId == claim.Value, null, "ApplicationUser");
            }
            else
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(null, null, "ApplicationUser");
            }

            if (status == "cancelled")
            {
                orderHeaders = orderHeaders.Where(o => o.Status == SD.StatusCancelled || o.Status == SD.StatusRefunded || o.Status == SD.PaymentStatusRejected);
            }
            else
            {
                if (status == "completed")
                {
                    orderHeaders = orderHeaders.Where(o => o.Status == SD.StatusCompleted);
                }
                else if (status == "inprocess")
                {
                    orderHeaders = orderHeaders.Where(o => o.Status == SD.StatusReady || o.Status == SD.StatusInProcess || o.Status == SD.PaymentStatusPending || o.Status == SD.StatusSubmitted);
                }
            }

            foreach (OrderHeader item in orderHeaders)
            {
                OrderDetailsVM orderDetailsVM = new OrderDetailsVM
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(p => p.OrderId == item.Id).ToList()
                };
                orderDetailsVMs.Add(orderDetailsVM);
            }

            return Json(new { data = orderDetailsVMs });
        }
    }
}
