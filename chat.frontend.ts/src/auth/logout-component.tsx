import { FC, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import authClient from "../api/clients/auth-client";
import { deleteToken } from "../api/local-storage/local-storage";
import useAuthContext from "../hooks/useAuthContext";


const LogoutComponent : FC<{}> = () => {
    const { setCurrentUser } = useAuthContext();
    const navigate = useNavigate();

    useEffect(() => {
        authClient.logout();
        deleteToken();
        setCurrentUser(null);
        navigate("/auth/login");
    }, [setCurrentUser, navigate]);

    return (<div>Выход...</div>);
}

export default LogoutComponent;