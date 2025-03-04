import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import './NewsEditor.css';
import ReactMarkdown from 'react-markdown';

import {
    Bold, Italic, List, ListOrdered, Image, Link,
    Code, Eye, Edit2, Save, ArrowLeft, HelpCircle
} from 'lucide-react';
import Container from "../../components/Layouts/Container/Container";

const NewsEditPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const isNewNews = id === 'new';

    const [viewMode, setViewMode] = useState('edit'); // 'edit' or 'preview'
    const [showHints, setShowHints] = useState(false);

    const [newsData, setNewsData] = useState({
        id: isNewNews ? Date.now() : parseInt(id),
        title: '',
        subtitle: '',
        content: '',
        author: '',
        date: new Date().toLocaleString('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' }),
        likes: 0,
        dislikes: 0,
        views: 0
    });

    const markdownHints = [
        { label: 'Заголовок', syntax: '# Заголовок', example: '# Главный заголовок' },
        { label: 'Подзаголовок', syntax: '## Подзаголовок', example: '## Секция статьи' },
        { label: 'Жирный', syntax: '**текст**', example: '**важная информация**' },
        { label: 'Курсив', syntax: '*текст*', example: '*выделенная мысль*' },
        { label: 'Список', syntax: '- Элемент списка', example: '- Первый пункт\n- Второй пункт' },
        { label: 'Нумерованный список', syntax: '1. Элемент списка', example: '1. Шаг первый\n2. Шаг второй' },
        { label: 'Ссылка', syntax: '[текст](url)', example: '[Наш сайт](https://example.com)' },
        { label: 'Изображение', syntax: '![alt-текст](url)', example: '![Логотип компании](logo.png)' },
        { label: 'Цитата', syntax: '> текст', example: '> Важная цитата из статьи' },
        { label: 'Код', syntax: '`код`', example: '`const x = 10;`' },
        { label: 'Блок кода', syntax: '```\nкод\n```', example: '```\nfunction example() {\n  return true;\n}\n```' },
    ];

    useEffect(() => {
        if (!isNewNews) {
            const allNews = JSON.parse(localStorage.getItem('newsData') || '[]');
            const currentNewsItem = allNews.find(item => item.id === parseInt(id));

            if (currentNewsItem) {
                setNewsData(currentNewsItem);
            } else {
                navigate('/');
            }
        }
    }, [id, isNewNews, navigate]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setNewsData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const insertFormatting = (formatType) => {
        const textarea = document.getElementById('content-textarea');
        if (!textarea) return;

        const start = textarea.selectionStart;
        const end = textarea.selectionEnd;
        const selectedText = newsData.content.substring(start, end);

        let insertText = '';

        switch (formatType) {
            case 'bold':
                insertText = `**${selectedText || 'жирный текст'}**`;
                break;
            case 'italic':
                insertText = `*${selectedText || 'курсивный текст'}*`;
                break;
            case 'list':
                insertText = selectedText
                    ? selectedText.split('\n').map(line => `- ${line}`).join('\n')
                    : '- Элемент списка\n- Элемент списка\n- Элемент списка';
                break;
            case 'ordered-list':
                insertText = selectedText
                    ? selectedText.split('\n').map((line, i) => `${i+1}. ${line}`).join('\n')
                    : '1. Первый элемент\n2. Второй элемент\n3. Третий элемент';
                break;
            case 'image':
                insertText = `![Описание изображения](url-изображения)`;
                break;
            case 'link':
                insertText = `[${selectedText || 'текст ссылки'}](https://example.com)`;
                break;
            case 'code':
                insertText = selectedText ? `\`${selectedText}\`` : '`код`';
                break;
            default:
                return;
        }

        const newContent = newsData.content.substring(0, start) + insertText + newsData.content.substring(end);

        setNewsData(prev => ({
            ...prev,
            content: newContent
        }));

        // Установка позиции курсора после вставки форматирования
        setTimeout(() => {
            textarea.focus();
            const newCursorPos = start + insertText.length;
            textarea.setSelectionRange(newCursorPos, newCursorPos);
        }, 0);
    };

    const saveNews = () => {
        const allNews = JSON.parse(localStorage.getItem('newsData') || '[]');

        if (isNewNews) {
            const newItem = {
                ...newsData,
                id: Date.now(),
                date: new Date().toLocaleString('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' }),
                likes: 0,
                dislikes: 0,
                views: 0,
                excerpt: newsData.content.substring(0, 150) + '...'
            };

            localStorage.setItem('newsData', JSON.stringify([...allNews, newItem]));
        } else {
            const updatedNews = allNews.map(item =>
                item.id === parseInt(id)
                    ? {
                        ...item,
                        title: newsData.title,
                        subtitle: newsData.subtitle,
                        content: newsData.content,
                        author: newsData.author,
                        excerpt: newsData.content.substring(0, 150) + '...'
                    }
                    : item
            );

            localStorage.setItem('newsData', JSON.stringify(updatedNews));
        }
        navigate('/');
    };

    const handleDeleteNews = () => {
        if (window.confirm('Вы уверены, что хотите удалить эту новость?')) {
            const allNews = JSON.parse(localStorage.getItem('newsData') || '[]');
            const updatedNews = allNews.filter(item => item.id !== parseInt(id));
            localStorage.setItem('newsData', JSON.stringify(updatedNews));
            navigate('/');
        }
    };

    return (
        <Container>
            <div className="news-editor-page">
                <div className="editor-header">
                    <div className="header-actions">
                        <button
                            className="back-button"
                            onClick={() => navigate(`/news/${id}`)}
                        >
                            <ArrowLeft size={18} /> Назад к новостям
                        </button>
                        {!isNewNews && (
                            <button
                                className="delete-button"
                                onClick={handleDeleteNews}
                            >
                                Удалить
                            </button>
                        )}
                    </div>
                    <h2>{isNewNews ? 'Создание новости' : 'Редактирование новости'}</h2>
                    <div className="view-mode-toggle">
                        <button
                            className={`mode-button ${viewMode === 'edit' ? 'active' : ''}`}
                            onClick={() => setViewMode('edit')}
                        >
                            <Edit2 size={18} /> Редактировать
                        </button>
                        <button
                            className={`mode-button ${viewMode === 'preview' ? 'active' : ''}`}
                            onClick={() => setViewMode('preview')}
                        >
                            <Eye size={18} /> Предпросмотр
                        </button>
                    </div>
                </div>

                {viewMode === 'edit' && (
                    <div className="edit-mode">
                        <div className="editor-form">
                            <div className="form-group">
                                <label htmlFor="title">Заголовок</label>
                                <input
                                    type="text"
                                    id="title"
                                    name="title"
                                    value={newsData.title}
                                    onChange={handleInputChange}
                                    placeholder="Введите заголовок новости"
                                    required
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="subtitle">Подзаголовок</label>
                                <input
                                    type="text"
                                    id="subtitle"
                                    name="subtitle"
                                    value={newsData.subtitle}
                                    onChange={handleInputChange}
                                    placeholder="Введите подзаголовок (краткое описание)"
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="author">Автор</label>
                                <input
                                    type="text"
                                    id="author"
                                    name="author"
                                    value={newsData.author}
                                    onChange={handleInputChange}
                                    placeholder="Имя автора"
                                />
                            </div>

                            <div className="form-group content-group">
                                <div className="content-label-wrapper">
                                    <label htmlFor="content">Содержание (Markdown)</label>
                                    <button
                                        className="hint-toggle"
                                        onClick={() => setShowHints(!showHints)}
                                        title="Показать/скрыть подсказки Markdown"
                                    >
                                        <HelpCircle size={16} />
                                        {showHints ? "Скрыть подсказки" : "Показать подсказки"}
                                    </button>
                                </div>

                                <div className="formatting-toolbar">
                                    <button onClick={() => insertFormatting('bold')} title="Жирный текст">
                                        <Bold size={18} />
                                    </button>
                                    <button onClick={() => insertFormatting('italic')} title="Курсивный текст">
                                        <Italic size={18} />
                                    </button>
                                    <button onClick={() => insertFormatting('list')} title="Маркированный список">
                                        <List size={18} />
                                    </button>
                                    <button onClick={() => insertFormatting('ordered-list')} title="Нумерованный список">
                                        <ListOrdered size={18} />
                                    </button>
                                    <button onClick={() => insertFormatting('image')} title="Изображение">
                                        <Image size={18} />
                                    </button>
                                    <button onClick={() => insertFormatting('link')} title="Ссылка">
                                        <Link size={18} />
                                    </button>
                                    <button onClick={() => insertFormatting('code')} title="Код">
                                        <Code size={18} />
                                    </button>
                                </div>

                                <textarea
                                    id="content-textarea"
                                    name="content"
                                    value={newsData.content}
                                    onChange={handleInputChange}
                                    placeholder="Введите содержание новости (поддерживается Markdown)"
                                    rows="15"
                                    required
                                ></textarea>
                            </div>

                            {showHints && (
                                <div className="markdown-hints">
                                    <h4>Подсказки по форматированию Markdown</h4>
                                    <div className="hints-grid">
                                        {markdownHints.map((hint, index) => (
                                            <div key={index} className="hint-item">
                                                <h5>{hint.label}</h5>
                                                <div className="hint-syntax">{hint.syntax}</div>
                                                <div className="hint-example">{hint.example}</div>
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            )}
                        </div>
                    </div>
                )}

                {viewMode === 'preview' && (
                    <div className="preview-mode">
                        <div className="preview-container">
                            <h1 className="preview-title">{newsData.title || 'Заголовок новости'}</h1>
                            {newsData.subtitle && <h2 className="preview-subtitle">{newsData.subtitle}</h2>}

                            {newsData.author && (
                                <div className="preview-meta">
                                    <span className="preview-author">Автор: {newsData.author}</span>
                                    <span className="preview-date">{newsData.date}</span>
                                </div>
                            )}

                            <div className="preview-content">
                                {newsData.content ? (
                                    <ReactMarkdown>{newsData.content}</ReactMarkdown>
                                ) : (
                                    <p className="preview-placeholder">Контент новости будет отображен здесь...</p>
                                )}
                            </div>
                        </div>
                    </div>
                )}

                <div className="editor-actions">
                    <button
                        className="save-button"
                        onClick={saveNews}
                        disabled={!newsData.title || !newsData.content}
                    >
                        <Save size={18} /> {isNewNews ? 'Опубликовать' : 'Сохранить изменения'}
                    </button>
                </div>
            </div>
        </Container>
    );
};

export default NewsEditPage;