import React, {useEffect} from "react";
import { RoomShortInfo } from "./RoomPage";
import { Room } from "../../models/room";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";

export function RoomList() {
    const dispatch = useActions();
    const rooms = useSelector<RootState, Room[]>((state) => state.room.rooms);;
    const history = useNavigate();

    useEffect(() => {
        dispatch.getAllRooms();
        }, [dispatch]);
    
    const handleClubsClick = (id: number) => {
        history(`/rooms/${id}`)
    };    

    return (
        <div className="short__room__info">
            {rooms.map(r => <RoomShortInfo room={r} onClick={() => handleClubsClick(r.id)} key={r.id}/>)}
        </div>
    )
}

export default RoomList;