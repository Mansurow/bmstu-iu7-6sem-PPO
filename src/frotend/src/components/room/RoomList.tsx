import React, {useEffect} from "react";
import { RoomShortInfo } from "./RoomPage";
import { Room } from "../../models/room";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import "./Room.css"

export function RoomList() {
    const dispatch = useActions();
    const rooms = useSelector<RootState, Room[]>((state) => state.room.rooms);;
    const history = useNavigate();

    useEffect(() => {
        dispatch.getAllRooms();
        }, [dispatch]);
    
    const handleRoomsClick = (id: number) => {
        history(`/rooms/${id}`)
    };    

    return (
        <div className="rooms__info">
            {rooms.map(r => <RoomShortInfo room={r} onClick={() => handleRoomsClick(r.id)} key={r.id}/>)}
        </div>
    )
}

export default RoomList;