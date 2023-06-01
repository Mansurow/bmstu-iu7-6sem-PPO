import { Booking } from "./booking";
import { UserType } from "./enums/usertype.enum";
import { Feedback } from "./feedback";
import { Menu } from "./menu";
import { Room } from "./room";
import { User } from "./user";

export interface RoomListProps {
    rooms: Room[],
    error: string | null
}

export interface BookingListProps {
    bookings: Booking[],
    error: string | null
}

export interface FeedbackListProps {
    feedbacks: Feedback[],
    error: string | null
}

export interface MenuListProps {
    menu: Menu[],
    error: string | null
}

export interface UserInfoListProps {
    users: User[],
    error: string | null;
}

export interface LoadingProps {
    isLoading: boolean
}

export interface UserFormProps {
    login: string;
    password: string;
}

export interface LoginProps {
    isLogin: boolean,
    role: UserType
}


