using Bike_Dekho.Data;
using Bike_Dekho.Models.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;

namespace Bike_Dekho.Models.Repository
{
    public class MakeRepo : IMakeRepo
    {
        private readonly AppDbContext dbContext;

        public MakeRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Make AddMake(Make Make)
        {
           dbContext.Makes.Add(Make);
            dbContext.SaveChanges();
            return Make;
        }

        public Make DeleteMake(int id)
        {
            Make make = dbContext.Makes.Find(id);
            if(make != null)
            {
                dbContext.Makes.Remove(make);
                dbContext.SaveChanges();
            }
            return make;
        }

        public Make GetMake(int id)
        {
            return dbContext.Makes.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Make> GetMakes()
        {
            return dbContext.Makes.ToList();
        }

        public Make UpdateMake(Make make)
        {
          var data=  dbContext.Makes.Find(make.Id);
            if(data != null)
            {
                data.Name=make.Name;
            }
            dbContext.SaveChanges();
            return data;
        }
    }
}
