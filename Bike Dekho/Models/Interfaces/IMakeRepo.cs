namespace Bike_Dekho.Models.Interfaces
{
    public interface IMakeRepo
    {
        public Make AddMake(Make Make);
        public Make UpdateMake(Make Make);
        public Make DeleteMake(int id);
        public IEnumerable<Make> GetMakes();
        public Make GetMake(int id);
    }
}
