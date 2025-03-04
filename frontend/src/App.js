import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import NewsPage from "./pages/NewsPage/NewsPage";
import NewsDetailPage from "./pages/NewsDetailsPage/NewsDetailPage";
import NewsEditPage from "./pages/NewsEditPage/NewsEditPage";
import AuthPage from "./pages/AuthPage/AuthPage";

function App() {
  return (
      <Router>
        <div className="app">
          <Routes>
            <Route path="/" element={<NewsPage />} />
            <Route path="/news/:id" element={<NewsDetailPage />} />
            <Route path="/news/edit/:id" element={<NewsEditPage />} />
            <Route path="/login" element={<AuthPage />} />
          </Routes>
        </div>
      </Router>
  );
}

export default App;
