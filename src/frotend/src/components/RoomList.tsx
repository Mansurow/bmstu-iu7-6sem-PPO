import React, {useEffect, useState} from "react";
import { RoomInfo } from "./RoomPage";
import { Room } from "../models/room";
import RoomService from "../services/http-service/room.service";

export function RoomList() {
    const [rooms, setRooms] = useState<Room[]>([]);

    useEffect(() => {
        RoomService.getAllRooms().then((data) => {
            setRooms(data);
        })
    })

    return (
        <div className="short__room__info">
            {rooms.map(r => <RoomInfo room={r} key={r.id}/>)}
        </div>
    )
}

export default RoomList;