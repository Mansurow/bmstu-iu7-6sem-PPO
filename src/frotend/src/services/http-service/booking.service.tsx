import axios, { AxiosResponse } from "axios";
import { Booking } from "../../models/booking";
import ResourseService from "../resourse.service";
import { BookingStatus } from "../../models/enums/bookingstatus.enum";

export default class BookingService {
    static api = '/bookings';

    static async getAllBookings(): Promise<Booking[]> {
        return await axios.get(ResourseService.url + this.api);
    }

    static async getUserBookings(userId: number): Promise<Booking[]> {
        return await axios.get(ResourseService.url + this.api + `${userId}`);
    }

    static async getBookingById(id: number): Promise<Booking> {
        return await axios.get(ResourseService.url + this.api + `/${id}`);
    }

    static async createBooking(booking: Booking): Promise<AxiosResponse<void>>
    {
        return await axios.post(ResourseService.url + this.api, booking);
    }

    static async deleteBooking(id: number): Promise<AxiosResponse<void>>
    {
        return await axios.delete(ResourseService.url + this.api + `${id}`);
    }
}