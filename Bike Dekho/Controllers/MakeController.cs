using Bike_Dekho.Models;
using Bike_Dekho.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bike_Dekho.Controllers
{
    [Authorize(Roles ="Admin, Executive")]
    public class MakeController : Controller
    {
        private readonly IMakeRepo makeRepo;

        public MakeController(IMakeRepo makeRepo)
        {
            this.makeRepo = makeRepo;
        }

        public IActionResult Index()
        {
            var makes= makeRepo.GetMakes();
            return View(makes);
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Make make)
        {
            if(ModelState.IsValid)
            {
                makeRepo.AddMake(make);
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            
            if(id==null)
            {
                return NotFound();
            }
            makeRepo.DeleteMake(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model= makeRepo.GetMake(id);
            if(model==null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Make make)
        {
            if(!ModelState.IsValid)
            {
                return View(make);
            }
            makeRepo.UpdateMake(make);
            return RedirectToAction("Index");
        }
    }
}
