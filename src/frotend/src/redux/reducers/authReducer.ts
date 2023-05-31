import { UserType } from "../../models/enums/usertype.enum";
import { LoginProps } from "../../models/types";
import { SIGN_IN } from "../constants/authConstants";


const initialLoginState : LoginProps =  {
    role: UserType.NoAuth,
    isLogin:false
};


const authReducer = (state = initialLoginState, action: any) => {
    switch (action.type) {
        case SIGN_IN:
            return {
                ...state,
                isLogin:action.payload.data.isLogin,
                role:action.payload.data.role
            }
        default:
            return state;
    }
};

export default authReducer;
