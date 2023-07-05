using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bike_Dekho.Models.Interfaces
{
    public interface IBikeRepo
    {
        public IEnumerable<Bikes> GetBikes();
        public Bikes GetBike(int id);
        public Bikes AddBike(Bikes bike);
        public Bikes UpdateBike(Bikes bike);
        public Bikes DeleteBike(int id);
        public IEnumerable<SelectListItem> MakeList();
        public IEnumerable<SelectListItem> ModelList(int makeId);
    }
}
