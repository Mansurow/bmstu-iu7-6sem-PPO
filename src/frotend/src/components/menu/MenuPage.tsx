import React, { useEffect, useState } from "react";
import { Room } from "../../models/room";
import { InventoryInfo } from "../InventoryInfo";
import { useNavigate, useParams } from "react-router-dom";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";
import { LoginProps } from "../../models/types";
import "./Menu.css"
import { UserType } from "../../models/enums/usertype.enum";
import { Menu } from "../../models/menu";

interface MenuProps {
    dish: Menu,
    onClick: () => void;
}

export const MenuShortInfo: React.FC<MenuProps> = ({dish, onClick}) => {
    return (
        <div className="short__menu__info " onClick={onClick}>
            <div className="menu__image">
                <img src="/img/no-image.jpg" alt="" />
            </div>
            <h3>{dish.name}</h3>
            <p>Цена: <span>{dish?.price}</span></p>
            <p>Тип: <span>{dish?.type}</span></p>
            <p>Описание: <span>{dish?.description}</span></p>
        </div>
    )
}

export const MenuPage: React.FC<LoginProps> = ({role, isLogin}) => 
{
    const dispatch = useActions();
    const {id} = useParams();
    const dish = useSelector<RootState, Menu | undefined>(state => state.menu.menu?.find(el => el.id === Number(id)));
    const history = useNavigate();
    
    useEffect(() => {
        dispatch.getAllMenu();
        }, [dispatch]);
    
    const [isEditing, setIsEditing] = useState(false);    
    const [editedName, setEditedName] = useState(dish?.name);
    const [editedPrice, setEditedPrice] = useState(dish?.price);
    const [editedType, setEditedType] = useState(dish?.type);    
    const [editedDescription, setEditedDescription] = useState(dish?.description);    

    const handleEditClick = () => {
        setIsEditing(true);
    };    

    const handleDeleteClick = () => {
        dispatch.deleteDish(Number(id));
        history('/menu')
    }

    const handleSaveClick = () => {
        const updateDish: Menu = {
            id: dish?.id!,
            name: editedName!,
            price: editedPrice!,
            type: editedType!,
            description: editedDescription!
        };

        dispatch.updateDish(Number(id), updateDish);
        setIsEditing(false);
    };

    return (
        <div className="full__room__info">
            { dish != undefined ? 
                <>
                    <div className="room__image">
                        <img src="/img/no-image.jpg" alt="" />
                    </div>
                    {
                        isEditing ? 
                        <>
                            <p>Название:</p><input type="text" value={editedName} onChange={(e) => setEditedName(e.target.value)} />
                        </>
                        : 
                        <h1>"{dish?.name}"</h1>
                    }   
                    <p>Цена: {isEditing ? <input type="text" value={editedPrice} onChange={(e) => setEditedPrice(Number(e.target.value))} /> : dish?.price} рублей</p>
                    <p>Описание: {isEditing ? <input type="text" value={editedDescription} onChange={(e) => setEditedDescription(e.target.value)} /> : dish?.description} кв. метров</p>
                    <div className="button-group">
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

export default MenuPage;
