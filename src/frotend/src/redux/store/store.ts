import { applyMiddleware, createStore } from 'redux';
import { combineReducers } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import thunk from 'redux-thunk';

import { LoadingProps, LoginProps, RoomListProps } from "../../models/types";
import roomReducer from "../reducers/roomReducer";

export interface AppState {
    room: RoomListProps
}

const rootReducer = combineReducers<AppState>({
    room: roomReducer
});

export const store = createStore(rootReducer, undefined, composeWithDevTools(applyMiddleware(thunk)))
export type RootState = ReturnType<typeof rootReducer>