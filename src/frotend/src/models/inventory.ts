import { Room } from "./room";

export interface Inventory {
    id: number,
    name: string,
    rooms: Room[]
}