import axios, { AxiosResponse } from "axios";
import ResourseService from "../resourse.service";
import { Room } from "../../models/room";

export default class RoomService {
    static api = '/rooms';

    static async getAllRooms(): Promise<Room[]> {
        return await axios.get(ResourseService.url + this.api);
        
    }

    static async getRoomById(id: number): Promise<Room> {
        return await axios.get(ResourseService.url + this.api + `/${id}`);
        
    }
}