import { Routes, Route, Navigate } from "react-router-dom"
import { useContext } from "react";
import ProtectedRoute from "./ProtectedRoute"
import SignInPage from "../pages/SignInPage/SignInPage"
import NavigationBar from "../components/NavigationBar/NavigationBar";

import { UserContext } from '../contexts/UserContext';


const AppRoutes = () => {
    const {currentUser} = useContext(UserContext);
    return (
        <Routes>
            <Route element={<ProtectedRoute/>} >
                <Route path="/" element={<NavigationBar/>}>
                    <Route path="/" element={<SignInPage></SignInPage>} />
                    <Route path="/subjects" element={<h1>grgr</h1>}  />
                    <Route path="/my-subjects" element={<h1>grgr</h1>}  />
                    <Route path="/all-subjects" element={<h1>grgr</h1>}  /> 
                </Route>
            </Route>
           <Route path="/signin" element={currentUser ? <Navigate to="/"/> : <SignInPage/>} />
        </Routes>
    )
}

export default AppRoutes