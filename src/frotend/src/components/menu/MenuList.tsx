import React, {useEffect} from "react";
import { useActions } from "../../hooks/useActions";
import { RootState } from "../../redux/store/store";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import "./Menu.css"
import { Menu } from "../../models/menu";
import { MenuShortInfo } from "./MenuPage";

export function MenuList() {
    const dispatch = useActions();
    const menu = useSelector<RootState, Menu[]>((state) => state.menu.menu);;
    const history = useNavigate();

    useEffect(() => {
        dispatch.getAllMenu();
        }, [dispatch]);
    
    const handleMenuClick = (id: number) => {
        history(`/menu/${id}`)
    };    

    return (
        <div className="rooms__info">
            {menu.map(d => <MenuShortInfo dish={d} onClick={() => handleMenuClick(d.id)} key={d.id}/>)}
        </div>
    )
}

export default MenuList;