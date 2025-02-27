import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Container from "../../components/Layouts/Container/Container";
import NewsList from "../../components/News/NewsList/NewsList";

const demoNews = [
    {
        id: 1,
        title: 'Новые технологии в разработке',
        subtitle: 'Как искусственный интеллект меняет индустрию',
        excerpt: 'В последние годы мы наблюдаем революционные изменения в области искусственного интеллекта...',
        content: `
      <p>В последние годы мы наблюдаем революционные изменения в области искусственного интеллекта. Компании по всему миру активно внедряют ИИ-решения в свои бизнес-процессы, что позволяет автоматизировать рутинные задачи и высвободить человеческие ресурсы для более творческой работы.</p>
      
      <p>Искусственный интеллект находит применение в самых разных сферах: от анализа данных и прогнозирования до обработки естественного языка и компьютерного зрения. Благодаря этому компании могут более эффективно работать с большими объемами информации, принимать взвешенные решения и улучшать пользовательский опыт.</p>
      
      <p>Особенно значительный прогресс наблюдается в области генеративных моделей, которые способны создавать новый контент: тексты, изображения, музыку и даже код. Эти модели открывают новые горизонты для творческих индустрий и разработки программного обеспечения.</p>
    `,
        date: '15 февраля 2025',
        author: 'Иван Петров',
        likes: 345,
        dislikes: 12,
        views: 1200
    },
    {
        id: 2,
        title: 'Запуск нового продукта',
        subtitle: 'Компания АЛЬФА представляет инновационное решение',
        excerpt: 'Сегодня компания АЛЬФА анонсировала выпуск нового продукта, который изменит взгляд на...',
        content: `
      <p>Сегодня компания АЛЬФА анонсировала выпуск нового продукта, который изменит взгляд на современные технологии. После трех лет разработки и тестирования, инновационное решение готово к выходу на массовый рынок.</p>
      
      <p>Новый продукт представляет собой интегрированную платформу для управления бизнес-процессами с использованием искусственного интеллекта и машинного обучения. Благодаря уникальным алгоритмам, система способна адаптироваться к конкретным потребностям бизнеса и оптимизировать рабочие процессы в реальном времени.</p>
      
      <p>По словам генерального директора компании, это решение позволит клиентам сократить операционные расходы на 30% и повысить производительность труда на 25%. Первые клиенты уже смогли оценить преимущества платформы во время бета-тестирования.</p>
      
      <p>Официальный запуск продукта запланирован на март 2025 года. Компания АЛЬФА уже принимает предварительные заказы на официальном сайте.</p>
    `,
        date: '20 февраля 2025',
        author: 'Мария Сидорова',
        likes: 278,
        dislikes: 15,
        views: 890
    },
    {
        id: 3,
        title: 'Мероприятие для разработчиков',
        subtitle: 'Открыта регистрация на ежегодную конференцию',
        excerpt: 'Приглашаем всех разработчиков принять участие в ежегодной конференции, которая пройдет...',
        content: `
      <p>Приглашаем всех разработчиков принять участие в ежегодной конференции, которая пройдет 10-12 апреля 2025 года в Москве. Это крупнейшее событие в IT-индустрии, собирающее экспертов и энтузиастов из разных областей разработки.</p>
      
      <p>В программе конференции запланированы выступления ведущих специалистов из России и зарубежных стран, мастер-классы, панельные дискуссии и хакатон с внушительным призовым фондом. Основные темы мероприятия: искусственный интеллект, облачные технологии, безопасность и новые тренды в веб-разработке.</p>
      
      <p>Участники смогут не только получить ценные знания и навыки, но и расширить свою профессиональную сеть, встретиться с потенциальными работодателями и партнерами. Для студентов и начинающих разработчиков предусмотрена специальная программа менторства.</p>
      
      <p>Регистрация открыта на официальном сайте мероприятия. Количество мест ограничено, поэтому рекомендуем зарегистрироваться заранее.</p>
    `,
        date: '12 февраля 2025',
        author: 'Алексей Кузнецов',
        likes: 120,
        dislikes: 5,
        views: 450
    }
];

const NewsPage = () => {
    const [news, setNews] = useState([]);
    const navigate = useNavigate();

    // Загружаем демо-данные при первом рендеринге
    useEffect(() => {
        setNews(demoNews);

        // Сохраняем полные данные в localStorage для доступа на странице детальной информации
        localStorage.setItem('newsData', JSON.stringify(demoNews));
    }, []);

    const handleSearch = (searchText) => {
        // Фильтруем новости по поисковому запросу
        if (!searchText.trim()) {
            setNews(demoNews);
            return;
        }

        const filteredNews = demoNews.filter((item) =>
            item.title.toLowerCase().includes(searchText.toLowerCase()) ||
            item.subtitle.toLowerCase().includes(searchText.toLowerCase()) ||
            item.excerpt.toLowerCase().includes(searchText.toLowerCase())
        );

        setNews(filteredNews);
    };

    const handleNewsClick = (id) => {
        navigate(`/news/${id}`);
    };

    return (
        <Container>
            <div className="news-page">
                <div className="news-header">
                    <h2 className="news-title">Новости</h2>
                    {/*<SearchBar onSearch={handleSearch} />*/}
                </div>

                <NewsList
                    news={news}
                    onNewsClick={handleNewsClick}
                />
            </div>
        </Container>
    );
};

export default NewsPage;