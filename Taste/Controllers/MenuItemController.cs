using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(new { data = _unitOfWork.MenuItem.GetAll(null,null,"Category,FoodType") });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var MenuItemObj = _unitOfWork.MenuItem.GetFirstOrDefault(p => p.Id == id);
                if (MenuItemObj == null)
                {
                    return Json(new { success = false, message = "Error While Deleting" });
                }

                var ImagePath = Path.Combine(_hostingEnvironment.WebRootPath, MenuItemObj.Image.TrimStart('\\'));
                if (System.IO.File.Exists(ImagePath))
                {
                    System.IO.File.Delete(ImagePath);
                }
                _unitOfWork.MenuItem.Remove(MenuItemObj);
                //_unitOfWork.MenuItem.RemoveFromList(MenuItemObj);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

            return Json(new { success = true, message = "Delete Successful" });
        }
    }
}
