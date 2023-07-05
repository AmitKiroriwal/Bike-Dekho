using Bike_Dekho.Models;
using Bike_Dekho.Models.Interfaces;
using Bike_Dekho.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Hosting.Internal;
using System.Data;
using System.Diagnostics;

namespace Bike_Dekho.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IBikeRepo bikeRepo;
        private readonly IModelRepo modelRepo;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment,IBikeRepo bikeRepo, IModelRepo modelRepo )
        {
            _logger = logger;
            this.webHostEnvironment = webHostEnvironment;
            this.bikeRepo= bikeRepo;
            this.modelRepo= modelRepo;
        }

        public IActionResult Index()
        {
            var model = bikeRepo.GetBikes();
            return View(model);
                    
        }
        public IActionResult BikeLists(string searchTerm, string sortBy, int page = 1)
        {
            //var model = bikeRepo.GetBikes();
            //return View(model);
            var bikes = bikeRepo.GetBikes();

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                bikes = bikes.Where(b => b.Make.Name.Contains(searchTerm) ||
                                         b.Model.Name.Contains(searchTerm) ||
                                         b.SellerName.Contains(searchTerm) ||
                                         b.SellerEmail.Contains(searchTerm) ||
                                         b.SellerPhone.Contains(searchTerm));
            }

            // Apply sorting
            switch (sortBy)
            {
                case "name":
                    bikes = bikes.OrderBy(b => b.Make.Name);
                    break;
                case "price":
                    bikes = bikes.OrderBy(b => b.Price);
                    break;
                // Add more sorting options if needed
                default:
                    bikes = bikes.OrderBy(b => b.Id);
                    break;
            }

            // Pagination
            const int pageSize = 10;
            var totalItems = bikes.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var bikesPaginated = bikes.Skip((page - 1) * pageSize).Take(pageSize);

            // Set the necessary ViewBag properties for the view
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;

            return View(bikesPaginated);

        }
        public async Task<IActionResult> FetchModel(int id)
        {
            var state =  bikeRepo.ModelList(id);
            return Json(state);
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Executive")]
        public async Task<IActionResult> Create()
        {
            ViewBag.MakeId = bikeRepo.MakeList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BikesViewModel bikes)
        {
            if (bikes != null)
            {
                string uniqeFileName = null;

                if (bikes.Photo != null)
                {
                    string upload = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    uniqeFileName = Guid.NewGuid().ToString() + "-" + bikes.Photo.FileName;
                    string photopath = Path.Combine(upload, uniqeFileName);
                    bikes.Photo.CopyTo(new FileStream(photopath, FileMode.Create));
                }
                Bikes bike = new Bikes()
                {
                    MakeId = bikes.MakeId,
                    ModelId = bikes.ModelId,
                    Mileage = bikes.Mileage,
                    Currency = bikes.Currency,
                    Year = bikes.Year,
                    Features = bikes.Features,
                    SellerName = bikes.SellerName,
                    SellerEmail = bikes.SellerEmail,
                    SellerPhone = bikes.SellerPhone,
                    Price = bikes.Price,
                    ImagePath = uniqeFileName
                };

                bikeRepo.AddBike(bike);
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
            bikeRepo.DeleteBike(id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            ViewBag.MakeId = bikeRepo.MakeList();
            ViewBag.ModelId = modelRepo.ModelList();
            Bikes bikes = bikeRepo.GetBike(id);
            if (bikes != null)
            {
                BikeEditViewModel bikeEditViewModel = new BikeEditViewModel()
                {
                    ID = bikes.Id,
                    MakeId = bikes.MakeId,
                    ModelId = bikes.ModelId,
                    Currency = bikes.Currency,
                    Mileage = bikes.Mileage,
                    Features = bikes.Features,
                    SellerEmail = bikes.SellerEmail,
                    SellerName = bikes.SellerName,
                    SellerPhone = bikes.SellerPhone,
                    ExistingPhotoPath = bikes.ImagePath,
                    Price = bikes.Price,
                    Year = bikes.Year
                };
                return View(bikeEditViewModel);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task <IActionResult> Edit(BikeEditViewModel bikes)
        {
            if (bikes != null)
            {
                string uniqeFileName = null;
                Bikes bike = bikeRepo.GetBike(bikes.Id);

                bike.MakeId = bikes.MakeId;
                bike.ModelId = bikes.ModelId;
                bike.Mileage = bikes.Mileage;
                bike.Currency = bikes.Currency;
                bike.Year = bikes.Year;
                bike.Features = bikes.Features;
                bike.SellerName = bikes.SellerName;
                bike.SellerEmail = bikes.SellerEmail;
                bike.SellerPhone = bikes.SellerPhone;
                bike.Price = bikes.Price;
                   
                if (bikes.Photo != null)
                {
                    if(bikes.ExistingPhotoPath!=null)
                    {
                        string filePath = Path.Combine(webHostEnvironment.WebRootPath,
                           "Images", bikes.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);

                    }
                    string upload = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                    uniqeFileName = Guid.NewGuid().ToString() + "-" + bikes.Photo.FileName;
                    string photopath = Path.Combine(upload, uniqeFileName);
                    bikes.Photo.CopyTo(new FileStream(photopath, FileMode.Create));
                    
                }
                bike.ImagePath = uniqeFileName;


                bikeRepo.UpdateBike(bike);
                return RedirectToAction("Index");

            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}