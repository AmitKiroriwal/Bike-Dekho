using Bike_Dekho.Data;
using Bike_Dekho.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bike_Dekho.Models.Repository
{
    public class ModelRepo : IModelRepo
    {
        private readonly AppDbContext dbContext;

        public ModelRepo(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Model AddModel(Model model)
        {
            dbContext.Models.Add(model);
            dbContext.SaveChanges();
            return model;
        }

        public Model DeleteModel(int id)
        {
            Model model = dbContext.Models.Find(id);
            if(model!= null)
            {
                dbContext.Models.Remove(model);
                dbContext.SaveChanges();
            }
            return model;
        }

        public IEnumerable<Model> GetModel()
        {
            return dbContext.Models.Include(m=>m.Make).ToList();
        }

        public Model GetModel(int id)
        {
            return dbContext.Models.Include(m => m.Make).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<SelectListItem> MakeList()
        {
            var data = dbContext.Makes.Select(s => new { Name = s.Name, id = s.Id });
            var res = data.Select(x => new SelectListItem { Text = x.Name, Value = x.id.ToString() }).ToList();
            return res;
        }
        public IEnumerable<SelectListItem> ModelList()
        {
            var data = dbContext.Models.Include(x=>x.Make).Select(s => new { Name = s.Name, id = s.Id });
            var res = data.Select(x => new SelectListItem { Text = x.Name, Value = x.id.ToString() }).ToList();
            return res;
        }
        public Model UpdateModel(Model model)
        {

            var data = dbContext.Models.Find(model.Id);
            if (data != null)
            {
                data.Name = model.Name;

                data.MakeID = model.MakeID;
            }
            // var data =      dbContext.Models.Attach(model);
            //data.State=Microsoft.EntityFrameworkCore.EntityState.Modified;
            dbContext.SaveChanges();

            return model;
        }
    }
}
