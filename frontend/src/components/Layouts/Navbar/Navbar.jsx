import React from 'react';
import "./Navbar.css"
import {useNavigate} from "react-router-dom";

const Navbar = () => {

    const navigate = useNavigate();

    return (
        <nav className="navbar">
            <div className="container navbar-container">
                <h1 className="nav-logo" onClick={() => navigate("/")} >АЛЬФА</h1>
                <ul className="nav-list">
                    <li className="nav-list-element">
                        <a className="nav-link" onClick={() => navigate("/")}>Главная</a>
                    </li>
                    <li className="nav-list-element">
                        <a className="nav-link" onClick={() => navigate("/profile")} >Профиль</a>
                    </li>
                    <li className="nav-list-element">
                        <a className="nav-link nav-link-logout">Выйти</a>
                    </li>
                </ul>
            </div>
        </nav>
    );
};

export default Navbar;