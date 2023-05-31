import React from 'react';
import './App.css';
import RoomList from './components/room/RoomList';
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import Footer from './components/footer/Footer';
import Navbar from './components/navbar/Navbar';
import { UserType } from './models/enums/usertype.enum';
import RoomPage from './components/room/RoomPage';
import SignIn from './components/auth/SignIn';
import { useSelector } from 'react-redux';
import { LoginProps } from './models/types';
import { RootState } from './redux/store/store';
import SignOut from './components/auth/SignOut';
import MenuList from './components/menu/MenuList';
import MenuPage from './components/menu/MenuPage';
import SignUp from './components/auth/SignUp';
import UserInfo from './components/user/UserInfo';
import UserInfoList from './components/user/UserInfoList';

function App() {
  const auth = useSelector<RootState, LoginProps>(state => state.auth)
  return (
    <>
      <BrowserRouter>
        <Navbar role={auth.role} isLogin={auth.isLogin} />
        <Routes>
          <Route path="/" element={<RoomList/>}/>
          <Route path="/home" element={<RoomList/>}/>
          <Route path="/rooms" element={<RoomList/>}/>
          <Route path="/rooms/:id" element={<RoomPage role={auth.role} isLogin={auth.isLogin}/>}/>
          <Route path="/signIn" element={<SignIn/>}/>
          <Route path="/signUp" element={<SignUp/>}/>
          <Route path="/signOut" element={<SignOut/>}/>
          <Route path='/users/me' element={<UserInfo/>}/>
          <Route path='/users' element={<UserInfoList/>}/>
          <Route path='/menu' element={<MenuList/>}/>
          <Route path='/menu/:id' element={<MenuPage role={auth.role} isLogin={auth.isLogin}/>}/>
          <Route path='/rooms/bookings/'/>
        </Routes>
        <Footer />
      </BrowserRouter>
    </>
  )
}

export default App;
