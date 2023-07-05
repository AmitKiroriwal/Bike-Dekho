using Bike_Dekho.Data;
using Bike_Dekho.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bike_Dekho.Models.Repository
{
    public class BikeRepo : IBikeRepo
    {
        private readonly AppDbContext dbContext;

        public BikeRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Bikes AddBike(Bikes bike)
        {
            dbContext.Bikes.Add(bike);
            dbContext.SaveChanges();
            return bike;
        }

        public Bikes DeleteBike(int id)
        {
            Bikes model = dbContext.Bikes.Find(id);
            if (model != null)
            {
                dbContext.Bikes.Remove(model);
                dbContext.SaveChanges();
            }
            return model;
        }

        public Bikes GetBike(int id)
        {
            return dbContext.Bikes.Include(b=>b.Make).Include(m=>m.Model).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Bikes> GetBikes()
        {
            
            return dbContext.Bikes.Include(b => b.Make).Include(m => m.Model).ToList();
        }

        public IEnumerable<SelectListItem> MakeList()
        {
            var data = dbContext.Makes.Select(s => new { Name = s.Name, id = s.Id });
            var res = data.Select(x => new SelectListItem { Text = x.Name, Value = x.id.ToString() }).ToList();
            return res;
        }

        public IEnumerable<SelectListItem> ModelList(int makeId)
        {
            var data = dbContext.Models.Where(x=>x.MakeID==makeId).Select(s => new { Name = s.Name, id = s.Id });
            var res = data.Select(x => new SelectListItem { Text = x.Name, Value = x.id.ToString() }).ToList();
            return res;
        }

        public Bikes UpdateBike(Bikes bike)
        {
            
             var data =      dbContext.Bikes.Attach(bike);
            data.State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();

            return bike;
        }
    }
}
