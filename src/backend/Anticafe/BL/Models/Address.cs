namespace Anticafe.BL.Models;

public class Address
{
    public int Id { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Area { get; set; }
    public string Street { get; set; }
    public int NumberStreet { get; set; }
    public int NumberHouse { get; set; }

    Address(int id, string country, string city, string area, string street, int numberStreet, int numberHouse)
    {
        Id = id;
        Country = country;
        City = city;
        Area = area;
        Street = street;
        NumberStreet = numberStreet;
        NumberHouse = numberHouse;
    }
}
