import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './ProfilePage.css';

import {
    Key, Mail, User, Calendar, Edit, Lock, Shield, Check, X
} from 'lucide-react';
import Container from "../../components/Layouts/Container/Container";

const ProfilePage = () => {
    const navigate = useNavigate();
    const [userData, setUserData] = useState({
        id: 12345,
        username: 'johndoe',
        fullName: 'Иванов Иван Иванович',
        registrationDate: '2023-01-15',
        email: 'johndoe@example.com',
        role: 'user', // 'user' или 'admin'
        canPublishNews: false // true или false
    });

    // Функция для восстановления пароля
    const handlePasswordReset = () => {
        alert('Функция восстановления пароля в разработке...');
    };

    // Функция для привязки новой почты
    const handleBindEmail = () => {
        const newEmail = prompt('Введите новую почту:');
        if (newEmail) {
            setUserData(prev => ({ ...prev, email: newEmail }));
            alert('Почта успешно обновлена!');
        }
    };

    // Функция для изменения роли (только для администратора)
    const handleChangeRole = () => {
        if (userData.role === 'admin') {
            alert('Вы не можете изменить роль администратора.');
            return;
        }
        const newRole = userData.role === 'user' ? 'admin' : 'user';
        setUserData(prev => ({ ...prev, role: newRole }));
        alert(`Роль изменена на: ${newRole}`);
    };

    // Функция для изменения прав публикации новостей
    const handleTogglePublishRights = () => {
        setUserData(prev => ({ ...prev, canPublishNews: !prev.canPublishNews }));
        alert(`Права публикации новостей: ${!userData.canPublishNews ? 'да' : 'нет'}`);
    };

    return (
        <Container>
            <div className="profile-page">
                <h1>Мой профиль</h1>
                <div className="profile-info">
                    <div className="info-item">
                        <User size={18} />
                        <span>Идентификатор:</span>
                        <strong>{userData.id}</strong>
                    </div>
                    <div className="info-item">
                        <User size={18} />
                        <span>Имя пользователя:</span>
                        <strong>{userData.username}</strong>
                    </div>
                    <div className="info-item">
                        <User size={18} />
                        <span>ФИО:</span>
                        <strong>{userData.fullName}</strong>
                    </div>
                    <div className="info-item">
                        <Calendar size={18} />
                        <span>Дата регистрации:</span>
                        <strong>{userData.registrationDate}</strong>
                    </div>
                    <div className="info-item">
                        <Mail size={18} />
                        <span>Почта:</span>
                        <strong>{userData.email || 'Не указана'}</strong>
                    </div>
                    <div className="info-item">
                        <Shield size={18} />
                        <span>Роль в системе:</span>
                        <strong>{userData.role === 'admin' ? 'Администратор' : 'Пользователь'}</strong>
                    </div>
                    <div className="info-item">
                        <Edit size={18} />
                        <span>Права публикации новостей:</span>
                        <strong>{userData.canPublishNews ? 'Да' : 'Нет'}</strong>
                    </div>
                </div>

                <div className="profile-actions">
                    <button
                        className="action-button"
                        onClick={handlePasswordReset}
                    >
                        <Lock size={18} /> Восстановить пароль
                    </button>
                    <button
                        className="action-button"
                        onClick={handleBindEmail}
                    >
                        <Mail size={18} /> Привязать другую почту
                    </button>
                </div>
            </div>
        </Container>
    );
};

export default ProfilePage;