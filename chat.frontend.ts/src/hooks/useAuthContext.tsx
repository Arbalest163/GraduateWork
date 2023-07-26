import { useContext } from "react";
import AuthContext from "../auth/auth-context-provider";

const useAuthContext = () => {
    return(
        useContext(AuthContext)
    );
}

export default useAuthContext;