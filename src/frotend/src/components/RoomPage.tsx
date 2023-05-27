import React from "react";
import { Room } from "../models/room";
import { InventoryInfo } from "./InventoryInfo";

interface RoomProps {
    room: Room
}

export function RoomInfo({room} : RoomProps)
{
    return (
        <div className="item">
            <div className="item_image">
                <img src="/img/no-image.jpg" className="w-1/6" alt="" />
            </div>
            <h1>{room.name}</h1>
            {room.inventories.map(inv => <InventoryInfo inventory={inv} key={inv.id}/>)}
        </div>
    )
}

export default RoomInfo;