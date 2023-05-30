import {Dispatch} from "react";
import {SIGN_IN} from "../constants/authConstants";
import { UserFormProps } from "../../models/types";
import UserService from "../../services/http-service/user.service";
import { FETCH_USER } from "../constants/userConstants";


export const signIn = (user:UserFormProps) => async (dispatch:Dispatch<any>) => {
    try {
        const data=  await UserService.SignIn(user);

        dispatch({ type: FETCH_USER, payload:{data} });
        dispatch({ type: SIGN_IN, payload:{data:{data,isLogin:true}}});

    } catch (error) {
        console.log(error);
    }
};