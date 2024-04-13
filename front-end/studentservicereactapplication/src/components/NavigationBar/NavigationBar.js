import { Link, Outlet } from "react-router-dom";
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import { useEffect, useState } from "react";

const NavigationBar = () => {
    const {currentUser, setCurrentUser} = useContext(UserContext);

    const [role, setRole] = useState("");

    useEffect(() => {
        const [header, payload, signature] = currentUser.split('.');
        const decodedPayload = atob(payload);
        const payloadObj = JSON.parse(decodedPayload);
        const userRole = payloadObj["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        setRole(userRole)
    }, [])

    const handleSignOut = async () => {
        setCurrentUser(null)
    }

    return (
        <div>
            {role === "professor" ? 
            <nav>
                <ul>
                    <li id="1" >
                        <Link to={"/"} >My Profile</Link>
                    </li>
                    <li id="2" >
                        <Link to={"/subjects"} >Subjects</Link>
                    </li>
                    <li id="3" >
                        <Link to={"/signin"} onClick={handleSignOut} >Sign out!</Link>
                    </li>
                </ul>
            </nav> 
            : 
            <nav>
                <ul>
                    <li id="1" >
                        <Link to={"/"} >My Profile</Link>
                    </li>
                    <li id="2" >
                        <Link to={"/my-subjects"} >My Subjects</Link>
                    </li>
                    <li id="3" >
                        <Link to={"/all-subjects"} >All Subjects</Link>
                    </li>
                    <li id="4" >
                        <Link to={"/signin"} onClick={handleSignOut} >Sign out!</Link>
                    </li>
                </ul>
            </nav>}
            <Outlet/>
        </div>
    )
}

export default NavigationBar