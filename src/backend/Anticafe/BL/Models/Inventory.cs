using Anticafe.BL.Enums;

namespace Anticafe.BL.Models;

public class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Inventory(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
