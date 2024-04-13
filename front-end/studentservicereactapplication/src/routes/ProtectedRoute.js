import { Navigate, Outlet } from "react-router-dom";
import { useContext } from "react";
import { UserContext } from '../contexts/UserContext'



const ProtectedRoute = () => {
    const {currentUser} = useContext(UserContext);
    return currentUser ? <Outlet/> : <Navigate to="/signin" />
}

export default ProtectedRoute