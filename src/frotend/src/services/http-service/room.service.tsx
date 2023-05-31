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

    static async updateRoom(id: number, room: Room): Promise<AxiosResponse<void>>
    {
        return await axios.put(ResourseService.url + this.api + `${id}`, room);
    }

    static async createRoom(room: Room): Promise<AxiosResponse<void>>
    {
        return await axios.post(ResourseService.url + this.api, room);
    }

    static async deleteRoom(id: number): Promise<AxiosResponse<void>>
    {
        return await axios.delete(ResourseService.url + this.api + `${id}`);
    }
}