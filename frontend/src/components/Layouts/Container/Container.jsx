import React from 'react';
import Navbar from "../Navbar/Navbar";

const Container = ({ children }) => {
    return (
        <div className="layout">
            <Navbar />
            <main className="container">
                {children}
            </main>
        </div>
    );
};

export default Container;