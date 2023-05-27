import { DishType } from "./enums/dishtype.enum";

export interface Menu {
    id: number,
    name: string,
    type: DishType,
    price: number,
    description: string
}