import { Room } from "../../models/room";
import { RoomListProps } from "../../models/types";
import { CREATE_ROOM, EDIT_ROOM, FETCH_ALL_ROOM } from "../constants/roomConstanst";

const initialClubState: { rooms: Room[] } = {
    rooms: []
};

const clubReducer = (state = initialClubState, action: any): RoomListProps => {
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
        default:
            return <RoomListProps>state;
    }
};

export default clubReducer;