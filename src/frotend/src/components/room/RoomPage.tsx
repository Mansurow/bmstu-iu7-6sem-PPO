import React, { useEffect, useState } from "react";
import { Room } from "../../models/room";
import { InventoryInfo } from "../InventoryInfo";
import { useNavigate, useParams } from "react-router-dom";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";
import { LoginProps } from "../../models/types";
import "./Room.css"
import { UserType } from "../../models/enums/usertype.enum";

interface RoomProps {
    room: Room,
    onClick: () => void;
}

export const RoomShortInfo: React.FC<RoomProps> = ({room, onClick}) => {
    return (
        <div className="short__room__info " onClick={onClick}>
            <div className="room__image">
                <img src="/img/no-image.jpg" alt="" />
            </div>
            <h3>{room.name}</h3>
            <p>Цена: <span>{room?.price}</span></p>
            <p>Рейтинг: <span>{room?.rating}</span></p>
        </div>
    )
}

export const RoomPage: React.FC<LoginProps> = ({role, isLogin}) => 
{
    const dispatch = useActions();
    const {id} = useParams();
    const room = useSelector<RootState, Room | undefined>(state => state.room.rooms?.find(el => el.id === Number(id)));
    const history = useNavigate();
    
    useEffect(() => {
        dispatch.getAllRooms();
        }, [dispatch]);
    
    const [isEditing, setIsEditing] = useState(false);    
    const [editedName, setEditedName] = useState(room?.name);
    const [editedPrice, setEditedPrice] = useState(room?.price);
    const [editedSize, setEditedSize] = useState(room?.size);    
    const [editedInventories, setEditedInventories] = useState([]);    

    const handleEditClick = () => {
        setIsEditing(true);
    };    

    const handleBookingClick = () => {
        history(`/rooms/${id}/booking`)
    };

    const handleSignInClick = () => {
        history(`/signin`)
    };

    const handleDeleteClick = () => {
        dispatch.deleteRoom(Number(id));
        history('/rooms')
    }

    const handleSaveClick = () => {
        const updatedRoom: Room = {
            id: room?.id!,
            name: editedName!,
            price: editedPrice!,
            size: editedSize!,
            rating: room?.rating!,
            inventories: editedInventories
        };

        dispatch.updateRoom(Number(id), updatedRoom);
        setIsEditing(false);
    };

    return (
        <div className="full__room__info">
            {   room != undefined ? 
                <>
                    <div className="room__image">
                        <img src="/img/no-image.jpg" alt="" />
                    </div>
                    {
                        isEditing ? 
                        <>
                            <p>Название: <input type="text" value={editedName} onChange={(e) => setEditedName(e.target.value)} /></p>
                        </>
                        : 
                        <h1>"{room?.name}"</h1>
                    }  
                    { !isEditing &&
                        (room?.inventories.length ? room?.inventories.map(inv => <InventoryInfo inventory={inv} key={inv.id}/>)
                        : "Отсутсвует")
                    }
                    <p>Цена: {isEditing ? <input type="text" value={editedPrice} onChange={(e) => setEditedPrice(Number(e.target.value))} /> : room?.price} рублей</p>
                    <p>Площадь: {isEditing ? <input type="text" value={editedSize} onChange={(e) => setEditedSize(Number(e.target.value))} /> : room?.size} кв. метров</p>
                    {!isEditing && <p>Рейтинг: {room?.rating}/5</p>}
                    <div className="button-group">
                        {!isLogin && (
                            <button onClick={handleSignInClick}>
                                Войдите, чтобы забронировать
                            </button>
                        )}
                        {isLogin && role === UserType.User && (
                            <button onClick={handleBookingClick}>
                                Забронировать
                            </button>
                        )}
                        {!isEditing && role === UserType.Admin && (
                            <button onClick={handleEditClick}>
                                Edit
                            </button>
                        )}
                        {!isEditing && role === UserType.Admin && (
                            <button onClick={handleDeleteClick}>
                                Delete
                            </button>
                        )}
                        {isEditing && role === UserType.Admin && (
                            <button onClick={handleSaveClick}>
                                Save
                            </button>
                        )}
                    </div>
                </>
                : <p>"Комната не найдена"</p> 
            }
        </div>
    )
}

export default RoomPage;
