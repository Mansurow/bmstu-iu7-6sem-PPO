import axios, { AxiosResponse } from "axios";
import ResourseService from "../resourse.service";
import { Room } from "../../models/room";
import { type } from "os";
import { DishType } from "../../models/enums/dishtype.enum";
import { Menu } from "../../models/menu";

export default class MenuService {
    static api = '/menu';
    static async getAllMenu(): Promise<Menu[]> {
        return await axios.get(ResourseService.url + this.api);
    }

    static async getDishById(id: number): Promise<Menu> {
        return await axios.get(ResourseService.url + this.api + `/${id}`);
    }

    static async updateDish(id: number, dish: Menu): Promise<AxiosResponse<void>>
    {
        return await axios.put(ResourseService.url + this.api + `${id}`, dish);
    }

    static async createDish(dish: Menu): Promise<AxiosResponse<void>>
    {
        return await axios.post(ResourseService.url + this.api, dish);
    }

    static async deleteDish(id: number): Promise<AxiosResponse<void>>
    {
        return await axios.delete(ResourseService.url + this.api + `${id}`);
    }
}