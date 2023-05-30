import { LoginProps } from "../../models/types";
import { SIGN_IN } from "../constants/authConstants";


const initialLoginState : LoginProps =  {
    role: undefined,
    isLogin:false
};


const authReducer = (state = initialLoginState, action: any) => {
    switch (action.type) {
        case SIGN_IN:
            return {
                ...state,
                isLogin:action.payload.data.isLogin,
                role:action.payload.data.data.user.role[0].permission
            }
        default:
            return state;
    }
};

export default authReducer;
