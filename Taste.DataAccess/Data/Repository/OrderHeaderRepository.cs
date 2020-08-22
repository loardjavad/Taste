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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(OrderHeader orderHeader)
        {
            var orderHeaderObj = _dbContext.OrderHeaders.FirstOrDefault(m => m.Id == orderHeader.Id);
            orderHeaderObj.UpdateTime = orderHeader.UpdateTime;
            _dbContext.OrderHeaders.Update(orderHeaderObj);

            _dbContext.SaveChanges();
        }
    }
}
