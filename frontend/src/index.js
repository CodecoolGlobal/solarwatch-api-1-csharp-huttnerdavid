import React from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";
import ReactDOM from 'react-dom/client';
import './index.css';
import { useState } from 'react';

import Layout from "./Pages/Layout";
import Home from "./Pages/Home";
import Register from "./Pages/Register";
import Login from "./Pages/Login";
import SolarWatch from './Pages/SolarWatch';
import PrivateRoute from './Components/PrivateRoute';

export default function App() {
  const [isLoggedIn, setLogin] = useState(false);

  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout isLoggedIn={isLoggedIn}/>}>
          <Route index element={<Home isLoggedIn={isLoggedIn}/>}/>
          <Route path="solar-watch" element={<PrivateRoute element={<SolarWatch/>} isLoggedIn={isLoggedIn}/>}/>
          <Route path="registration" element={<Register />}/>
          <Route path="login" element={<Login setLogin={setLogin}/>}/>
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(<App />);