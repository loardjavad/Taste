using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Taste.Models
{
   public class MenuItem:BaseEntities
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="Price should be greater than $1")]
        public double Price { get; set; }

        [Display(Name ="Category Type")]
        public long CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Category Type")]
        public long FoodTypeId { get; set; }

        [ForeignKey("FoodTypeId")]
        public virtual FoodType FoodType { get; set; }
    }
}
