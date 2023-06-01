import { Dispatch } from "react";
import { UserFormProps } from "../../models/types";
import { User } from "../../models/user";
import UserService from "../../services/http-service/user.service";
import { SIGN_UP } from "../constants/authConstants";
import { EDIT_USER, FETCH_ALL_USER } from "../constants/userConstants";

// Action Creators
export const userAction = (updatedUser: User) => {
    return {
        type: EDIT_USER,
        payload: updatedUser,
    };
};

export const getAllUsers = () => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await UserService.GetAllUsers();

        dispatch({ type: FETCH_ALL_USER, payload: { data } });

    } catch (error) {
        console.log(error)
    }
}

