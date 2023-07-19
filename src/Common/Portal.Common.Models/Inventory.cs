namespace Portal.Common.Models;

public class Inventory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly YearOfProduction { get; set; }

    public Inventory(Guid id, string name, string description, DateOnly yearOfProduction)
    {
        Id = id;
        Name = name;
        Description = description;
        YearOfProduction = yearOfProduction;
    }
}
