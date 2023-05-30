import { Dispatch } from "react";
import { UserFormProps } from "../../models/types";
import { User } from "../../models/user";
import UserService from "../../services/http-service/user.service";
import { SIGN_UP } from "../constants/authConstants";
import { EDIT_USER } from "../constants/userConstants";

// Action Creators
export const userAction = (updatedUser: User) => {
    return {
        type: EDIT_USER,
        payload: updatedUser,
    };
};

export const createUser = (user: UserFormProps) => async (dispatch:Dispatch<any>) => {
    try {
        // dispatch({type:START_LOADING})

        const data = await UserService.SignUp(user);
        dispatch({type:SIGN_UP, payload:data})
        // dispatch(({type:END_LOADING}))
    } catch (error) {
        console.log(error)
    }
}
