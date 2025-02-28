import React from 'react';
import "Navbar.css"

const Navbar = () => {
    return (
        <nav className="navbar">
            <div className="container navbar-container">
                <h1 className="nav-logo">АЛЬФА</h1>
                <ul className="nav-list">
                    <li className="nav-list-element">
                        <a className="nav-link">Главная</a>
                    </li>
                    <li className="nav-list-element">
                        <a className="nav-link">О нас</a>
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