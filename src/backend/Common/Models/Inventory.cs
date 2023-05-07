using Anticafe.Common.Enums;

namespace Anticafe.BL.Models;

public class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Room>? Rooms { get; set; }

    public Inventory(int id, string name, ICollection<Room>? rooms)
    {
        Id = id;
        Name = name;
        Rooms = rooms;
    }

    public Inventory(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
