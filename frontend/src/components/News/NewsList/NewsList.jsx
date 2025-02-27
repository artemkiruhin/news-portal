import React from 'react';
import NewsItem from "../NewsItem/NewsItem";

const NewsList = ({ news, onNewsClick }) => {
    return (
        <div className="news-list">
            {news && news.length > 0 ? (
                news.map((item) => (
                    <NewsItem
                        key={item.id}
                        news={item}
                        onClick={() => onNewsClick(item.id)}
                    />
                ))
            ) : (
                <div className="news-empty">
                    <p>Новости не найдены</p>
                </div>
            )}
        </div>
    );
};

export default NewsList;