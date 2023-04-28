using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Anticafe.DataAccess.DBModels;

public class InventoryDbModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; }
    public ICollection<RoomDbModel>? Rooms { get; set; }

    public InventoryDbModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public InventoryDbModel(int id, string name, ICollection<RoomDbModel>? rooms)
    {
        Id = id;
        Name = name;
        Rooms = rooms;
    }
}
