import React from 'react';

const Container = ({ children }) => {
    return (
        <div className="layout">
            {/*<Navbar />*/}
            <main className="container">
                {children}
            </main>
        </div>
    );
};

export default Container;