import { Routes, Route, Navigate } from "react-router-dom"
import { useContext } from "react";
import ProtectedRoute from "./ProtectedRoute"
import SignInPage from "../pages/SignInPage/SignInPage"
import NavigationBar from "../components/NavigationBar/NavigationBar";

import { UserContext } from '../contexts/UserContext';
import SignUpPage from "../pages/SignUpPage/SignUpPage";
import ProfilePage from "../pages/ProfilePage/ProfilePage"
import AthendedSubjectsPage from "../pages/AthendedSubjectsPage/AthendedSubjectsPage";
import UnAthendedSubjectsPage from "../pages/UnAthendedSubjectsPage/UnAthendedSubjectsPage";
import SubjectPage from "../pages/SubjectsPage/SubjectPage";


const AppRoutes = () => {
    const {currentUser} = useContext(UserContext);
    return (
        <Routes>
            <Route element={<ProtectedRoute/>} >
                <Route path="/" element={<NavigationBar/>}>
                    <Route path="/" element={<ProfilePage/>} />
                    <Route path="/subjects" element={<SubjectPage/>}  />
                    <Route path="/athended-subjects" element={<AthendedSubjectsPage/>}  />
                    <Route path="/unathended-subjects" element={<UnAthendedSubjectsPage/>}  /> 
                </Route>
            </Route>
           <Route path="/signin" element={currentUser ? <Navigate to="/"/> : <SignInPage/>} />
           <Route path="/signup" element={<SignUpPage/>} />
        </Routes>
    )
}

export default AppRoutes