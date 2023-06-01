import { Booking } from "../../models/booking";
import { BookingListProps } from "../../models/types";
import { CREATE_BOOKING, FETCH_ALL_BOOKING } from "../constants/bookingConstants";
import { DELETE_FEEDBACK } from "../constants/feedbackConstants";

const initialBookingState: { bookings: Booking[]} = {
    bookings: [],
};
const bookingReducer = (state = initialBookingState, action: any): BookingListProps => {
    switch (action.type) {
        case FETCH_ALL_BOOKING:
            return {
                ...state,
                bookings:action.payload.data,
                error:null
            }   
        case CREATE_BOOKING:
            return {
                ...state,
                bookings:[...state.bookings, action.payload.data],
                error:null
            }
        case DELETE_FEEDBACK:
            return {
                ...state,
                bookings:[...state.bookings, action.payload.data],
                error:null
            }    
        default:
            return <BookingListProps> state;
    }
};

export default bookingReducer;