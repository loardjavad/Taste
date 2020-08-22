using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Taste.Models
{
   public class FoodType:BaseEntities
    {
        [Required]
        [Display(Name = "FoodType Name")]
        public string Name { get; set; }
    }
}
