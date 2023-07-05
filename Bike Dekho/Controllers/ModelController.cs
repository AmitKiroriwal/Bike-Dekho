using Bike_Dekho.Models;
using Bike_Dekho.Models.Interfaces;
using Bike_Dekho.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Bike_Dekho.Controllers
{
    [Authorize(Roles = "Admin, Executive")]
    public class ModelController : Controller
    {
        private readonly IModelRepo modelRepo;

        public ModelController(IModelRepo modelRepo)
        {
            this.modelRepo = modelRepo;
        }

        public IActionResult Index()
        { 
            var model= modelRepo.GetModel();
            return View(model);
        }
        public IActionResult Add()
        {
            ViewBag.Makes = modelRepo.MakeList();
            return View();
        }
        [HttpPost]
        public IActionResult Add(Model model)
        {
            if (model!=null)
            {
                modelRepo.AddModel(model);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            modelRepo.DeleteModel(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Makes = modelRepo.MakeList();
            var model = modelRepo.GetModel(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Model model)
        {
            if (model==null)
            {
                return View();
            }
            modelRepo.UpdateModel(model);
            return RedirectToAction("Index");
        }
    }
}
