import { BookingStatus } from "./enums/bookingstatus.enum";

export interface Booking {
    id: number,
    room_id: number,
    user_id: number,
    amount_people: number,
    status: BookingStatus,
    start_time: string,
    end_time: string
}