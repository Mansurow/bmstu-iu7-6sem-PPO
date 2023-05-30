import { Dispatch } from "react"
import { Room } from "../../models/room"
import { EDIT_ROOM, FETCH_ALL_ROOM } from "../constants/roomConstanst"
import RoomService from "../../services/http-service/room.service"

// ACTION Creators
export const roomAction = (updateRoom: Room) => {
    return {
        type: EDIT_ROOM,
        payload: updateRoom
    }
}

export const getAllClubs = () => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await RoomService.getAllRooms();

        dispatch({ type: FETCH_ALL_ROOM, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}