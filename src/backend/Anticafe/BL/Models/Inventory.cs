using Anticafe.BL.Enums;

namespace Anticafe.BL.Models;


public class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public InventoryStatus Status { get; set; }
    public DateTime ReleaseYear { get; set; }
    public ICollection<Room>? Rooms { get; set; }

    Inventory(int id, string name, InventoryStatus status, DateTime releaseYear, ICollection<Room>? rooms)
    {
        Id = id;
        Name = name;
        Status = status;
        ReleaseYear = releaseYear;
        Rooms = rooms;
    }
}
