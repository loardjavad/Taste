using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Taste.Data;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(OrderDetails orderDetails)
        {
            var orderDetailObj = _dbContext.OrderDetails.FirstOrDefault(m => m.Id == orderDetails.Id);
            orderDetailObj.UpdateTime = orderDetailObj.UpdateTime;
            _dbContext.OrderDetails.Update(orderDetailObj);

            _dbContext.SaveChanges();
        }
    }
}
