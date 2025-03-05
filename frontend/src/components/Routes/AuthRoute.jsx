import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const AuthRoute = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchAuth = async () => {
            // stub
            setIsAuthenticated(true);
            setIsLoading(false);
        };

        fetchAuth();
    }, []);

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            navigate("/login");
        }
    }, [isLoading, isAuthenticated, navigate]);

    if (isLoading) {
        return <div>Загрузка</div>;
    }

    if (!isAuthenticated) {
        navigate("/login");
    }

    return children;
};

export default AuthRoute;