import { applyMiddleware, createStore } from 'redux';
import { combineReducers } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import thunk from 'redux-thunk';

import { LoadingProps, LoginProps, RoomListProps } from "../../models/types";
import roomReducer from "../reducers/roomReducer";
import authReducer from '../reducers/authReducer';
import { User } from '../../models/user';
import userReducer from '../reducers/userReducer';
import UserService from '../../services/http-service/user.service';

export interface AppState {
    room: RoomListProps
    auth: LoginProps,
    currentUser: User,
}

const rootReducer = combineReducers<AppState>({
    room: roomReducer,
    auth: authReducer,
    currentUser: userReducer
});

export const store = createStore(rootReducer, undefined, composeWithDevTools(applyMiddleware(thunk)))
export type RootState = ReturnType<typeof rootReducer>