import { Room } from "../../models/room";
import { RoomListProps } from "../../models/types";
import { CREATE_ROOM, DELETE_ROOM, EDIT_ROOM, FETCH_ALL_ROOM, FETCH_ROOM } from "../constants/roomConstanst";

const initialRoomState: { rooms: Room[]} = {
    rooms: [],
};

const roomReducer = (state = initialRoomState, action: any): RoomListProps => {
    switch (action.type) {
        case FETCH_ALL_ROOM:
            return {
                ...state,
                rooms:action.payload.data,
                error:null
            }   
        case CREATE_ROOM:
            return {
                ...state,
                rooms:[...state.rooms, action.payload.data],
                error:null
            }
        case EDIT_ROOM:
            return {
                ...state,
                rooms:state.rooms.map(el => el.id === action.payload.data.id ? action.payload.data : el),
                error:null
            }
        case DELETE_ROOM:
            return {
                ...state,
                rooms:[...state.rooms, action.payload.data],
                error:null
            }    
        default:
            return <RoomListProps> state;
    }
};

export default roomReducer;