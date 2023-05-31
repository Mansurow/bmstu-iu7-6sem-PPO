import { applyMiddleware, createStore } from 'redux';
import { combineReducers } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import thunk from 'redux-thunk';

import { BookingListProps, LoadingProps, LoginProps, MenuListProps, RoomListProps, UserInfoListProps } from "../../models/types";
import roomReducer from "../reducers/roomReducer";
import authReducer from '../reducers/authReducer';
import { User } from '../../models/user';
import menuReducer from '../reducers/menuReducer';
import { usersReducer, userReducer} from '../reducers/userReducer';
import bookingReducer from '../reducers/bookingReducer';


export interface AppState {
    room: RoomListProps,
    menu: MenuListProps,
    auth: LoginProps,
    user: UserInfoListProps,
    booking: BookingListProps,
    currentUser: User,
}

const rootReducer = combineReducers<AppState>({
    room: roomReducer,
    menu: menuReducer,
    auth: authReducer,
    user: usersReducer,
    booking: bookingReducer,
    currentUser: userReducer
});

export const store = createStore(rootReducer, undefined, composeWithDevTools(applyMiddleware(thunk)))
export type RootState = ReturnType<typeof rootReducer>
