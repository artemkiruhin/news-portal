import React from 'react';

const NewsItem = ({ news, onClick }) => {
    console.log("Полученная новость в NewsItem:", news);

    if (!news) {
        return <div className="news-item">Загрузка данных...</div>;
    }

    return (
        <div className="news-item" onClick={onClick}>
            <div className="news-item-content">
                <h3 className="news-title">{news.title}</h3>
                <h4 className="news-subtitle">{news.subtitle}</h4>
                <p className="news-excerpt">{news.excerpt}</p>
            </div>
            <div className="news-reactions">
                <div className="reaction-item">
                    <span className="reaction-icon like-icon">👍</span>
                    <span className="reaction-count">{news.likes}</span>
                </div>
                <div className="reaction-item">
                    <span className="reaction-icon dislike-icon">👎</span>
                    <span className="reaction-count">{news.dislikes}</span>
                </div>
                <div className="reaction-item">
                    <span className="reaction-icon view-icon">👁️</span>
                    <span className="reaction-count">{news.views}</span>
                </div>
            </div>
        </div>
    );
};

export default NewsItem;