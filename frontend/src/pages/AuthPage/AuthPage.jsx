// AuthPage.jsx
import React, { useState } from 'react';
import './AuthPage.css';

const AuthPage = () => {
    const [credentials, setCredentials] = useState({
        email: '',
        password: ''
    });
    const [errors, setErrors] = useState({});
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCredentials(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const validateForm = () => {
        const newErrors = {};
        if (!credentials.email) {
            newErrors.email = 'Введите email';
        } else if (!/\S+@\S+\.\S+/.test(credentials.email)) {
            newErrors.email = 'Неверный формат email';
        }
        if (!credentials.password) {
            newErrors.password = 'Введите пароль';
        }
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (validateForm()) {
            setIsSubmitting(true);
            // Здесь логика авторизации
            console.log('Форма отправлена', credentials);
            setIsSubmitting(false);
        }
    };

    return (
        <div className="auth-page">
            <div className="container">
                <div className="auth-card">
                    <h1 className="auth-title">Вход в систему</h1>

                    <form onSubmit={handleSubmit} className="auth-form">
                        <div className={`form-group ${errors.email ? 'has-error' : ''}`}>
                            <label htmlFor="email">Электронная почта</label>
                            <input
                                type="email"
                                id="email"
                                name="email"
                                value={credentials.email}
                                onChange={handleInputChange}
                                className="form-control"
                                placeholder="Введите ваш email"
                            />
                            {errors.email && <span className="error-message">{errors.email}</span>}
                        </div>

                        <div className={`form-group ${errors.password ? 'has-error' : ''}`}>
                            <label htmlFor="password">Пароль</label>
                            <input
                                type="password"
                                id="password"
                                name="password"
                                value={credentials.password}
                                onChange={handleInputChange}
                                className="form-control"
                                placeholder="Введите ваш пароль"
                            />
                            {errors.password && <span className="error-message">{errors.password}</span>}
                        </div>

                        <button
                            type="submit"
                            className="auth-button"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? 'Вход...' : 'Войти'}
                        </button>

                        <div className="auth-links">
                            <a href="/forgot-password">Забыли пароль?</a>
                            {/*<span>•</span>*/}
                            {/*<a href="/signup">Регистрация</a>*/}
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default AuthPage;