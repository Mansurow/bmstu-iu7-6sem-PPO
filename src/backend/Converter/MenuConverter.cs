using Anticafe.BL.Models;
using Anticafe.DataAccess.DBModels;

namespace Anticafe.DataAccess.Converter;

public static class MenuConverter
{
    public static Menu ConvertDbModelToAppModel(MenuDbModel menu) 
    {
        return new Menu(id: menu.Id,
                        name: menu.Name,
                        type: menu.Type,
                        price: menu.Price,
                        description: menu.Description);
    }

    public static MenuDbModel ConvertAppModelToDbModel(Menu menu)
    {
        return new MenuDbModel(id: menu.Id,
                        name: menu.Name,
                        type: menu.Type,
                        price: menu.Price,
                        description: menu.Description);
    }
}
