import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NewsPage from "./pages/NewsPage/NewsPage";
import NewsDetailPage from "./pages/NewsDetailsPage/NewsDetailPage";
import NewsEditPage from "./pages/NewsEditPage/NewsEditPage";
import AuthPage from "./pages/AuthPage/AuthPage";
import ProfilePage from "./pages/ProfilePage/ProfilePage";
import AuthRoute from "./components/Routes/AuthRoute";
import AdminPanel from "./pages/AdminPanel/AdminPanel";

function App() {
    return (
        <Router>
            <div className="app">
                <Routes>
                    <Route path="/" element={<AuthRoute> <NewsPage /> </AuthRoute> }/>
                    <Route path="/profile" element={<AuthRoute><ProfilePage /></AuthRoute>} />
                    <Route path="/news/edit/:id" element={ <AuthRoute> <NewsEditPage /> </AuthRoute> } />
                    <Route path="/admin" element={ <AuthRoute> <AdminPanel /> </AuthRoute> } />

                    <Route path="/news/:id" element={<NewsDetailPage />} />
                    <Route path="/login" element={<AuthPage />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;