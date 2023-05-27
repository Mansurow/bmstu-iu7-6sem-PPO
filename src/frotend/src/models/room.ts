import { Inventory } from "./inventory";

export interface Room {
    id: number,
    name: string,
    size: number,
    price: number,
    rating: number,
    inventories: Inventory[]
}