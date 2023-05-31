import { Menu } from "../../models/menu";
import { MenuListProps } from "../../models/types";
import { CREATE_DISH, DELETE_DISH, EDIT_DISH, FETCH_ALL_MENU } from "../constants/menuConstants";

const initialMenuState: { menu: Menu[]} = {
    menu: [],
};

const menuReducer = (state = initialMenuState, action: any): MenuListProps => {
    switch (action.type) {
        case FETCH_ALL_MENU:
            return {
                ...state,
                menu:action.payload.data,
                error:null
            }   
        case CREATE_DISH:
            return {
                ...state,
                menu:[...state.menu, action.payload.data],
                error:null
            }
        case EDIT_DISH:
            return {
                ...state,
                menu:state.menu.map(el => el.id === action.payload.data.id ? action.payload.data : el),
                error:null
            }
        case DELETE_DISH:
            return {
                ...state,
                menu:[...state.menu, action.payload.data],
                error:null
            }    
        default:
            return <MenuListProps> state;
    }
};

export default menuReducer;