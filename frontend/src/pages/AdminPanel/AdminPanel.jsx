import React, { useState } from 'react';
import './AdminPanel.css';
import Container from "../../components/Layouts/Container/Container";

const AdminPanel = () => {
    // Пример данных для списка пользователей
    const users = [
        { id: 1, username: 'user1', registeredDate: '2023-01-01', role: 'Администратор', hasPublishedRights: true },
        { id: 2, username: 'user2', registeredDate: '2023-02-15', role: 'Редактор', hasPublishedRights: false },
        { id: 3, username: 'user3', registeredDate: '2023-03-10', role: 'Пользователь', hasPublishedRights: false },
        { id: 1, username: 'user1', registeredDate: '2023-01-01', role: 'Администратор', hasPublishedRights: true },
        { id: 2, username: 'user2', registeredDate: '2023-02-15', role: 'Редактор', hasPublishedRights: false },
        { id: 3, username: 'user3', registeredDate: '2023-03-10', role: 'Пользователь', hasPublishedRights: false },
        { id: 1, username: 'user1', registeredDate: '2023-01-01', role: 'Администратор', hasPublishedRights: true },
        { id: 2, username: 'user2', registeredDate: '2023-02-15', role: 'Редактор', hasPublishedRights: false },
        { id: 3, username: 'user3', registeredDate: '2023-03-10', role: 'Пользователь', hasPublishedRights: false },
    ];

    // Пример данных для списка логов
    const initialLogs = [
        { id: 1, description: 'Пользователь вошел в систему', timestamp: '2023-10-01 12:00', level: 'Информация' },
        { id: 2, description: 'Неудачная попытка входа', timestamp: '2023-10-01 12:05', level: 'Ошибка' },
        { id: 3, description: 'Статья опубликована', timestamp: '2023-10-01 12:10', level: 'Информация' },
        { id: 4, description: 'Пользователь вышел из системы', timestamp: '2023-10-01 12:15', level: 'Информация' },
        { id: 5, description: 'Ошибка базы данных', timestamp: '2023-10-01 12:20', level: 'Ошибка' },
        { id: 1, description: 'Пользователь вошел в систему', timestamp: '2023-10-01 12:00', level: 'Информация' },
        { id: 2, description: 'Неудачная попытка входа', timestamp: '2023-10-01 12:05', level: 'Ошибка' },
        { id: 3, description: 'Статья опубликована', timestamp: '2023-10-01 12:10', level: 'Информация' },
        { id: 4, description: 'Пользователь вышел из системы', timestamp: '2023-10-01 12:15', level: 'Информация' },
        { id: 5, description: 'Ошибка базы данных', timestamp: '2023-10-01 12:20', level: 'Ошибка' },
        { id: 1, description: 'Пользователь вошел в систему', timestamp: '2023-10-01 12:00', level: 'Информация' },
        { id: 2, description: 'Неудачная попытка входа', timestamp: '2023-10-01 12:05', level: 'Ошибка' },
        { id: 3, description: 'Статья опубликована', timestamp: '2023-10-01 12:10', level: 'Информация' },
        { id: 4, description: 'Пользователь вышел из системы', timestamp: '2023-10-01 12:15', level: 'Информация' },
        { id: 5, description: 'Ошибка базы данных', timestamp: '2023-10-01 12:20', level: 'Ошибка' },
    ];

    const [logs, setLogs] = useState(initialLogs);
    const [sortConfig, setSortConfig] = useState({ key: null, direction: 'asc' });
    const [filterLevel, setFilterLevel] = useState('Все');

    // Функция для сортировки логов
    const sortedLogs = React.useMemo(() => {
        let sortableLogs = [...logs];
        if (sortConfig.key) {
            sortableLogs.sort((a, b) => {
                if (a[sortConfig.key] < b[sortConfig.key]) {
                    return sortConfig.direction === 'asc' ? -1 : 1;
                }
                if (a[sortConfig.key] > b[sortConfig.key]) {
                    return sortConfig.direction === 'asc' ? 1 : -1;
                }
                return 0;
            });
        }
        return sortableLogs;
    }, [logs, sortConfig]);

    // Функция для фильтрации логов по уровню
    const filteredLogs = React.useMemo(() => {
        if (filterLevel === 'Все') {
            return sortedLogs;
        }
        return sortedLogs.filter(log => log.level === filterLevel);
    }, [sortedLogs, filterLevel]);

    // Функция для изменения сортировки
    const requestSort = (key) => {
        let direction = 'asc';
        if (sortConfig.key === key && sortConfig.direction === 'asc') {
            direction = 'desc';
        }
        setSortConfig({ key, direction });
    };

    return (
        <Container>
            <div className="admin-panel">
                <h1>Панель администратора</h1>

                {/* Список пользователей */}
                <section className="user-list">
                    <h2>Пользователи</h2>
                    <div className="tile-container">
                        {users.map(user => (
                            <div key={user.id} className="tile" onClick={() => console.log('Выбран пользователь:', user.id)}>
                                <div className="tile-header">
                                    <span className="tile-title">{user.username}</span>
                                    <span className={`tile-role ${user.role.toLowerCase()}`}>{user.role}</span>
                                </div>
                                <div className="tile-content">
                                    <p><strong>Дата регистрации:</strong> {user.registeredDate}</p>
                                    <p><strong>Права на публикацию:</strong> {user.hasPublishedRights ? 'Да' : 'Нет'}</p>
                                </div>
                            </div>
                        ))}
                    </div>
                </section>

                {/* Список логов */}
                <section className="log-list">
                    <h2>Логи</h2>
                    <div className="filter-controls">
                        <label>
                            Фильтр по уровню:
                            <select value={filterLevel} onChange={(e) => setFilterLevel(e.target.value)}>
                                <option value="Все">Все</option>
                                <option value="Информация">Информация</option>
                                <option value="Ошибка">Ошибка</option>
                            </select>
                        </label>
                    </div>
                    <table className="admin-table">
                        <thead>
                        <tr>
                            <th onClick={() => requestSort('id')}>
                                ID {sortConfig.key === 'id' ? (sortConfig.direction === 'asc' ? '▲' : '▼') : ''}
                            </th>
                            <th onClick={() => requestSort('description')}>
                                Описание {sortConfig.key === 'description' ? (sortConfig.direction === 'asc' ? '▲' : '▼') : ''}
                            </th>
                            <th onClick={() => requestSort('timestamp')}>
                                Время {sortConfig.key === 'timestamp' ? (sortConfig.direction === 'asc' ? '▲' : '▼') : ''}
                            </th>
                            <th onClick={() => requestSort('level')}>
                                Уровень {sortConfig.key === 'level' ? (sortConfig.direction === 'asc' ? '▲' : '▼') : ''}
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                        {filteredLogs.map(log => (
                            <tr key={log.id}>
                                <td>{log.id}</td>
                                <td>{log.description}</td>
                                <td>{log.timestamp}</td>
                                <td className={`log-level ${log.level.toLowerCase()}`}>{log.level}</td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </section>
            </div>
        </Container>
    );
};

export default AdminPanel;