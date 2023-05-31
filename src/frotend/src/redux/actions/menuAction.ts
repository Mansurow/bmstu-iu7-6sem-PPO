import { Dispatch } from "react";
import MenuService from "../../services/http-service/menu.service";
import { CREATE_DISH, DELETE_DISH, EDIT_DISH, FETCH_ALL_MENU, FETCH_DISH } from "../constants/menuConstants";
import { Menu } from "../../models/menu";
import RoomService from "../../services/http-service/room.service";

// ACTION Creators
export const dishAction = (updateDish: Menu) => {
    return {
        type: EDIT_DISH,
        payload: updateDish
    }
}

export const getAllMenu = () => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await MenuService.getAllMenu();

        dispatch({ type: FETCH_ALL_MENU, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

export const getDishById= (id:number) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await MenuService.getDishById(id);

        dispatch({ type: FETCH_DISH, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

export const updateDish = (id:number, dish:Menu) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await MenuService.updateDish(id, dish);
        dispatch({type:EDIT_DISH})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}

export const createDish = (dish:Menu) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await MenuService.createDish(dish);
        dispatch({type:CREATE_DISH})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}

export const deleteDish = (id: number) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        await MenuService.deleteDish(id);
        dispatch({type:DELETE_DISH})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}