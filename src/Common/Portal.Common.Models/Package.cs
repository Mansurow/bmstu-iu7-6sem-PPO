using Portal.Common.Models.Enums;

namespace Portal.Common.Models
{
    public class Package
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PackageType Type { get; set; }
        public double Price { get; set; }
        public int RentalTime { get; set; }
        public string Description { get; set; }
        public ICollection<Zone> Zones { get; set; }
        public ICollection<Dish> Dishes { get; set; }

        public Package(Guid id, string name, PackageType type, double price, int rentalTime, string description, ICollection<Zone> zones, ICollection<Dish> dishes)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            RentalTime = rentalTime;
            Description = description;
            Zones = zones;
            Dishes = dishes;
        }
    }
}
