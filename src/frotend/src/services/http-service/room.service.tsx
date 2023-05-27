import axios from "axios";
import ResourseService from "../resourse.service";
import { Room } from "../../models/room";

export default class RoomService {
    private api = '/rooms';

    static MockRooms: Room[] =[
        {
            id: 1,
            name: "room1",
            size: 17,
            price: 2000,
            rating: 4.0,
            inventories: []
        },
        {
            id: 2,
            name: "room2",
            size: 17,
            price: 2000,
            rating: 4.0,
            inventories: []
        },
        {
            id: 3,
            name: "room3",
            size: 17,
            price: 2000,
            rating: 4.0,
            inventories: []
        }
    ]

    static async getAllRooms(): Promise<Room[]> {
        // return await axios.get(ResourseService.url + this.api);
        return this.MockRooms
    }

    static async getRoomById(id: number): Promise<Room> {
        // return await axios.get(ResourseService.url + this.api + `/${id}`);
        return this.MockRooms[0];
    }
}