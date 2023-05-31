import {Dispatch} from "react";
import {SIGN_IN, SIGN_OUT, SIGN_UP} from "../constants/authConstants";
import { UserFormProps } from "../../models/types";
import UserService from "../../services/http-service/user.service";
import { FETCH_USER } from "../constants/userConstants";
import { UserType } from "../../models/enums/usertype.enum";
import { User } from "../../models/user";


export const signIn = (user:UserFormProps) => async (dispatch:Dispatch<any>) => {
    try {
        const data = await UserService.SignIn(user);

        dispatch({ type: FETCH_USER, payload:{data} });
        dispatch({ type: SIGN_IN, payload:{data:{data,isLogin:true}}});

    } catch (error) {
        console.log(error);
    }
};

export const signOut = () => async (dispatch:Dispatch<any>) => {
    try {
        dispatch({ type: SIGN_OUT, payload:{data:{role:UserType.NoAuth,isLogin:false}}});

    } catch (error) {
        console.log(error);
    }
};


export const createUser = (user: User) => async (dispatch:Dispatch<any>) => {
    try {
        const data = await UserService.SignUp(user);
        dispatch({ type: SIGN_UP, payload:{data:{data,isLogin:true}}});

    } catch (error) {
        console.log(error);
    }
};