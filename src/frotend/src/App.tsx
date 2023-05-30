import React from 'react';
import './App.css';
import RoomList from './components/room/RoomList';
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import Footer from './components/footer/Footer';
import Navbar from './components/navbar/Navbar';
import { UserType } from './models/enums/usertype.enum';

function App() {
  return (
    <>
      <BrowserRouter>
        <Navbar role={UserType.User} isLogin={false} />
        <Routes>
          <Route path="/" element={<RoomList/>}/>
          <Route path="/home" element={<RoomList/>}/>
        </Routes>
        <Footer />
      </BrowserRouter>
    </>
  )
}

export default App;
