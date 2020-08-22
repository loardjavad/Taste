using System;
using System.Collections.Generic;
using System.Text;

namespace Taste.Models
{
    public abstract class BaseEntities<TKey>
    {
        public TKey Id { get; set; }
        public DateTime InsertTime { get; set; } = DateTime.Now;
        public bool IsRemoved { get; set; }
        public DateTime? RemoveTime { get; set; }
        public DateTime? UpdateTime { get; set; }

    }
    public abstract class BaseEntities : BaseEntities<long>
    {

    }
}
