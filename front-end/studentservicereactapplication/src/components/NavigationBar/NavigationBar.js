import { Link, Outlet } from "react-router-dom";
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import { useEffect, useState } from "react";
import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

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
            <AppBar position="static" >
                <Toolbar>
                    <Typography variant="h6"></Typography>
                    <Button color="inherit"><Link to={"/"} >My Profile</Link></Button>
                    <Button color="inherit"><Link to={"/subjects"} >Subjects</Link></Button>
                    <Button color="inherit"><Link to={"/signin"}  onClick={handleSignOut}  >Sign out!</Link></Button>
                </Toolbar>
                {/*<ul>
                    <li id="1" >
                        <Link to={"/"} >My Profile</Link>
                    </li>
                    <li id="2" >
                        <Link to={"/subjects"} >Subjects</Link>
                    </li>
                    <li id="3" >
                        <Link to={"/signin"}  onClick={handleSignOut}  >Sign out!</Link>
                    </li>
                </ul>
                */}
            </AppBar> 
            : 
            <AppBar  position="static">
                <Toolbar>
                    <Typography variant="h6"></Typography>
                    <Button color="inherit"><Link to={"/"} >My Profile</Link></Button>
                    <Button color="inherit"><Link to={"/athended-subjects"} >My Subjects</Link></Button>
                    <Button color="inherit"><Link to={"/unathended-subjects"} >All Subjects</Link></Button>
                    <Button color="inherit"><Link to={"/signin"} onClick={handleSignOut} >Sign out!</Link></Button>
                </Toolbar>
                {/*
                <ul>
                    <li id="1" >
                        <Link to={"/"} >My Profile</Link>
                    </li>
                    <li id="2" >
                        <Link to={"/athended-subjects"} >My Subjects</Link>
                    </li>
                    <li id="3" >
                        <Link to={"/unathended-subjects"} >All Subjects</Link>
                    </li>
                    <li id="4" >
                        <Link to={"/signin"} onClick={handleSignOut} >Sign out!</Link>
                    </li>
                </ul> 
                */}
            </AppBar>
            }
            <Outlet/>
        </div>
    )
}

export default NavigationBar