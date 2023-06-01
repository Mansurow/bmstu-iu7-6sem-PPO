import { Gender } from "../../models/enums/gender.enum";
import { UserType } from "../../models/enums/usertype.enum";
import { UserInfoListProps } from "../../models/types";
import { User } from "../../models/user";
import { FETCH_ALL_USER, FETCH_USER } from "../constants/userConstants";

const initialUserState: User = {
    id: 1,
    surname: "",
    name: "",
    firstname: "",
    gender: Gender.Unknown,
    birthday: "",
    email: "",
    password: "",
    role: UserType.NoAuth,
    phone: ""
};

const initializeUsersState:{users :User[]} = {
    users:[]
}

export const userReducer = (state = initialUserState, action: any): User => {
    switch (action.type) {
        case FETCH_USER:
            return {
                ...state,
                ...action.payload.data
            }
        default:
            return state;
    }
};

export const usersReducer = (state = initializeUsersState, action: any): UserInfoListProps => {
    switch (action.type) {
        case FETCH_ALL_USER:
            return {
                ...state,
                users:action.payload.data,
                error:null
            }
        default:
            return <UserInfoListProps> state;
    }
};

