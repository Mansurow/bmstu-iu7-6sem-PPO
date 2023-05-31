import { Dispatch } from "react";
import BookingService from "../../services/http-service/booking.service";
import { CREATE_BOOKING, DELETE_BOOKING, FETCH_ALL_BOOKING, FETCH_BOOKING } from "../constants/bookingConstants";
import { Booking } from "../../models/booking";

export const getAllBookings = () => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await BookingService.getAllBookings();

        dispatch({ type: FETCH_ALL_BOOKING, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

export const getBookingById= (id:number) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await BookingService.getBookingById(id);

        dispatch({ type: FETCH_BOOKING, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

export const createBooking = (booking: Booking) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await BookingService.createBooking(booking);
        dispatch({type:CREATE_BOOKING})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}

export const deleteBooking = (id: number) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await BookingService.deleteBooking(id);
        dispatch({type:DELETE_BOOKING})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}