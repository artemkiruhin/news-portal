import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import Container from "../../components/Layouts/Container/Container";

const NewsDetailPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [newsDetail, setNewsDetail] = useState(null);
    const [userReaction, setUserReaction] = useState(null);
    const [viewed, setViewed] = useState(false);

    useEffect(() => {
        // Получаем данные из localStorage
        const newsData = JSON.parse(localStorage.getItem('newsData') || '[]');
        const currentNews = newsData.find(item => item.id === parseInt(id));

        if (currentNews) {
            setNewsDetail(currentNews);

            if (!viewed) {
                currentNews.views += 1;
                setViewed(true);
                localStorage.setItem('newsData', JSON.stringify(
                    newsData.map(item => item.id === parseInt(id) ? currentNews : item)
                ));
            }

            const savedReactions = JSON.parse(localStorage.getItem('userReactions') || '{}');
            if (savedReactions[id]) {
                setUserReaction(savedReactions[id]);
            }
        } else {
            navigate('/');
        }
    }, [id, navigate, viewed]);

    const handleReaction = (type) => {
        if (!newsDetail) return;
        let updatedNews = { ...newsDetail };
        const savedReactions = JSON.parse(localStorage.getItem('userReactions') || '{}');

        if (type === 'like') {
            if (userReaction === 'like') {
                updatedNews.likes -= 1;
                setUserReaction(null);
                delete savedReactions[id];
            } else {
                if (userReaction === 'dislike') {
                    updatedNews.dislikes -= 1;
                    updatedNews.likes += 1;
                } else {
                    updatedNews.likes += 1;
                }
                setUserReaction('like');
                savedReactions[id] = 'like';
            }
        } else if (type === 'dislike') {
            if (userReaction === 'dislike') {
                updatedNews.dislikes -= 1;
                setUserReaction(null);
                delete savedReactions[id];
            } else {
                if (userReaction === 'like') {
                    updatedNews.likes -= 1;
                    updatedNews.dislikes += 1;
                } else {
                    updatedNews.dislikes += 1;
                }
                setUserReaction('dislike');
                savedReactions[id] = 'dislike';
            }
        }
        setNewsDetail(updatedNews);
        localStorage.setItem('userReactions', JSON.stringify(savedReactions));

        const newsData = JSON.parse(localStorage.getItem('newsData') || '[]');
        localStorage.setItem('newsData', JSON.stringify(
            newsData.map(item => item.id === parseInt(id) ? updatedNews : item)
        ));
    };

    const handleBack = () => {
        navigate('/');
    };

    const handleEdit = () => {
        navigate(`/news/edit/${id}`);
    };

    if (!newsDetail) {
        return (
            <Container>
                <div className="news-detail">
                    <div className="news-detail-loading">
                        <p>Загрузка новости...</p>
                    </div>
                </div>
            </Container>
        );
    }

    return (
        <Container>
            <div className="news-detail">
                <div className="news-detail-header">
                    <h1 className="news-detail-title">{newsDetail.title}</h1>
                    <h2 className="news-detail-subtitle">{newsDetail.subtitle}</h2>

                    <div className="news-detail-meta">
                        <div className="news-detail-date">
                            <span className="meta-icon">📅</span>
                            <span>{newsDetail.date}</span>
                        </div>
                        <div className="news-detail-author">
                            <span className="meta-icon">✍️</span>
                            <span>{newsDetail.author}</span>
                        </div>
                        <div className="news-detail-views">
                            <span className="meta-icon">👁️</span>
                            <span>{newsDetail.views}</span>
                        </div>
                    </div>
                </div>

                <div
                    className="news-detail-content"
                    dangerouslySetInnerHTML={{ __html: newsDetail.content }}
                />

                <div className="news-detail-reactions">
                    <button
                        className={`reaction-button like-button ${userReaction === 'like' ? 'active' : ''}`}
                        onClick={() => handleReaction('like')}
                    >
                        <span className="reaction-icon">👍</span>
                        <span className="reaction-count">{newsDetail.likes}</span>
                    </button>

                    <button
                        className={`reaction-button dislike-button ${userReaction === 'dislike' ? 'active' : ''}`}
                        onClick={() => handleReaction('dislike')}
                    >
                        <span className="reaction-icon">👎</span>
                        <span className="reaction-count">{newsDetail.dislikes}</span>
                    </button>
                </div>

                <div className="news-detail-actions">
                    <button className="news-detail-back" onClick={handleBack}>
                        <span className="back-icon">←</span>
                        <span>Вернуться к списку новостей</span>
                    </button>
                    <button className="news-detail-back" onClick={handleEdit}>
                        <span>Редактировать</span>
                    </button>
                </div>
            </div>
        </Container>
    );
};

export default NewsDetailPage;