using System;
using System.ComponentModel.DataAnnotations;


namespace Taste.Models
{
    public class Category : BaseEntities
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Display Order")]
        public string DisplayOrder { get; set; }
    }
}
