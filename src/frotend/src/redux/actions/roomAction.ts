import { Dispatch } from "react"
import { Room } from "../../models/room"
import { CREATE_ROOM, DELETE_ROOM, EDIT_ROOM, FETCH_ALL_ROOM, FETCH_ROOM } from "../constants/roomConstanst"
import RoomService from "../../services/http-service/room.service"

// ACTION Creators
export const roomAction = (updateRoom: Room) => {
    return {
        type: EDIT_ROOM,
        payload: updateRoom
    }
}

export const getAllRooms = () => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await RoomService.getAllRooms();

        dispatch({ type: FETCH_ALL_ROOM, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

export const getRoomById= (id:number) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await RoomService.getRoomById(id);

        dispatch({ type: FETCH_ROOM, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

export const updateRoom = (id:number, room:Room) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await RoomService.updateRoom(id, room);
        dispatch({type:EDIT_ROOM})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}

export const createRoom  = (room:Room) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await RoomService.createRoom(room);
        dispatch({type:CREATE_ROOM})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}

export const deleteRoom  = (id: number) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await RoomService.deleteRoom(id);
        dispatch({type:DELETE_ROOM})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}