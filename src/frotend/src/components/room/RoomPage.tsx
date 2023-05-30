import React, { useEffect } from "react";
import { Room } from "../../models/room";
import { InventoryInfo } from "../InventoryInfo";
import { useParams } from "react-router-dom";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";

interface RoomProps {
    room: Room,
    onClick: () => void;
}

export const RoomShortInfo: React.FC<RoomProps> = ({room, onClick}) => {
    return (
        <div className="full__room__info" onClick={onClick}>
            <div className="room__image">
                <img src="/img/no-image.jpg" className="w-1/6" alt="" />
            </div>
            <h3>{room.name}</h3>
            <p>Цена: <span>{room?.price}</span></p>
            <p>Рейтинг: <span>{room?.rating}</span></p>
        </div>
    )
}

export const RoomPage: React.FC = () => 
{
    const dispatch = useActions();
    const {id} = useParams();

    useEffect(() => {
        dispatch.getAllRooms();
        }, [dispatch]);
    
    const room = useSelector<RootState, Room | undefined>(state => state.room.rooms?.find(el => el.id === Number(id)));    

    return (
        <div className="full__room__info">
            {   room != undefined ? 
                <>
                    <div className="room__image">
                        <img src="/img/no-image.jpg" className="w-1/3" alt="" />
                    </div>
                    <h1>"{room?.name}"</h1>
                    {room?.inventories.length ? room?.inventories.map(inv => <InventoryInfo inventory={inv} key={inv.id}/>)
                    : "Отсутсвует"}
                    <p>Цена: {room?.price} рублей</p>
                    <p>Площадь: <span>{room?.size} кв. метров</span></p>
                    <p>Рейтинг: {room?.rating}/5</p>
                </>
                : <p>"Комната не найдена"</p> 
            }
        </div>
    )
}

export default RoomPage;
