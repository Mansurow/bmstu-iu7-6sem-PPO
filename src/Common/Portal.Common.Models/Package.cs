using Portal.Common.Models.Enums;

namespace Portal.Common.Models
{
    /// <summary>
    /// Пакет для зоны
    /// </summary>
    public class Package
    {
        /// <summary>
        /// Идентификатор 
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Название пакета
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Тип пакета
        /// </summary>
        public PackageType Type { get; set; }
        
        /// <summary>
        /// Цена пакета
        /// </summary>
        public double Price { get; set; }
        
        /// <summary>
        /// Общее время проведения
        /// </summary>
        public int RentalTime { get; set; }
        
        /// <summary>
        /// Описания пакета
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Зоны, в которых доступен данный пакет
        /// </summary>
        public ICollection<Zone> Zones { get; set; }
        
        /// <summary>
        /// Включенный список блюд в пакет 
        /// </summary>
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

        public Package(Guid id, string name, PackageType type, double price, int rentalTime, string description)
        {
            Id = id;
            Name = name;
            Type = type;
            Price = price;
            RentalTime = rentalTime;
            Description = description;
            Zones = new List<Zone>();
            Dishes = new List<Dish>();
        }
    }
}
