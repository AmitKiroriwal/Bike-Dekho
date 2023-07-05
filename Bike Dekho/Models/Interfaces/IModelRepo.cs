using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bike_Dekho.Models.Interfaces
{
    public interface IModelRepo
    {
        public IEnumerable<Model> GetModel();
        public Model GetModel(int id);
        public Model AddModel(Model model);
        public Model UpdateModel(Model model);
        public Model DeleteModel(int id);
        public IEnumerable<SelectListItem> MakeList();
        public IEnumerable<SelectListItem> ModelList();
    }
}
