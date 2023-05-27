import React from 'react';
import logo from './logo.svg';
import './App.css';
import RoomList from './components/RoomList';
import {BrowserRouter, Route, Routes} from 'react-router-dom';
import Header from './components/Header';
import Footer from './components/Footer';

function App() {
  return (
    <>
      <BrowserRouter>
        <Header />
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
