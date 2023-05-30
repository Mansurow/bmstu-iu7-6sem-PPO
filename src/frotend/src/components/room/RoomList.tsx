import React, {useEffect, useState} from "react";
import { RoomInfo } from "./RoomPage";
import { Room } from "../../models/room";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";

export function RoomList() {
    const dispatch = useActions();
    const rooms = useSelector<RootState, Room[]>((state) => state.room.rooms);;
    
    useEffect(() => {
        dispatch.getAllClubs();
        }, [dispatch]);

    return (
        <div className="short__room__info">
            {rooms.map(r => <RoomInfo room={r} key={r.id}/>)}
        </div>
    )
}

export default RoomList;