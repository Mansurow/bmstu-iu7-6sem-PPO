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

function App() {
  const auth = useSelector<RootState, LoginProps>(state => state.auth)
  return (
    <>
      <BrowserRouter>
        <Navbar role={UserType.User} isLogin={auth.isLogin} />
        <Routes>
          <Route path="/" element={<RoomList/>}/>
          <Route path="/home" element={<RoomList/>}/>
          <Route path="/rooms" element={<RoomList/>}/>
          <Route path="/rooms/:id" element={<RoomPage/>}/>
          <Route path="/signin" element={<SignIn/>}/>
          <Route path='/menu'/>
          <Route path='/rooms/bookings/'/>
        </Routes>
        <Footer />
      </BrowserRouter>
    </>
  )
}

export default App;
