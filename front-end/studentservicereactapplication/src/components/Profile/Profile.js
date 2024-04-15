import { useState, useEffect } from "react"
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Label from '@mui/material/Button';
import { GetStudent } from "../../services/StudentService";
import { GetProfesor, UpdateProfessor } from "../../services/ProfessorService";
import { UpdateStudent } from "../../services/StudentService";

const Profile = () => {
    const {currentUser} = useContext(UserContext);
    const [trigger, setTrigger] = useState(0);
    const [firstName, setFirstName] = useState("")
    const [lastName, setLastName] = useState("")
    const [indexNumber, setIndexNumber] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [role, setRole] = useState(null);
    const [user, setUser] = useState(null)

    useEffect(() => {
        const getUser = async () => {
            const [header, payload, signature] = currentUser.split('.');
            const decodedPayload = atob(payload);
            const payloadObj = JSON.parse(decodedPayload);
            const userRole = payloadObj["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
            setRole(userRole)
            if(userRole === "student")
            {
                const idxnmb = payloadObj["indexNumber"]
                setIndexNumber(idxnmb)
                const response = await GetStudent(idxnmb, currentUser)
                setUser(response.data)
                setFirstName(response.data.firstName)
                setLastName(response.data.lastName)
                setEmail(response.data.email)
                setIndexNumber(response.data.indexNumber)
                setPassword(response.data.password)
            }
            else
            {
                const eml = payloadObj["email"]
                setEmail(email)
                const response = await GetProfesor(eml, currentUser)
                setUser(response.data)
                setFirstName(response.data.firstName)
                setLastName(response.data.lastName)
                setEmail(response.data.email)
                setPassword(response.data.password)
            }
        }
        getUser()
        
    }, [])

    const handleUpdate = async () => {
            if(role === "student")
            {
                try
                {
                    const response = await UpdateStudent(user.id, firstName, lastName, indexNumber, email, password, currentUser);
                    console.log(response)
                    alert("Update successful!")
                }
                catch(err)
                {
                    if (!err?.response)
                        alert("No server response, login failed!");
                    else
                        alert(JSON.stringify(err.response.data));
                }
            }
            else
            {
                try
                {
                    const response = await UpdateProfessor(user.id, firstName, lastName, email, password, currentUser);
                    console.log(response)
                    alert("Update successful!")
                }
                catch(err)
                {
                    if (!err?.response)
                        alert("No server response, login failed!");
                    else
                        alert(JSON.stringify(err.response.data));
                }
            }
    }

    return (
        <div>
            {user && (
                <>
                    <Typography variant='h5' >Profile</Typography>
                    
                    <Label>First name:</Label>
                    <TextField required id='firstName' value={firstName} onChange={(e) => setFirstName(e.target.value)} ></TextField>
                    <Label>Last name:</Label>
                    <TextField required id='lastName' value={lastName}  onChange={(e) => setLastName(e.target.value)} ></TextField>
                    {indexNumber && (
                        <>
                            <Label>Index number:</Label>
                            <TextField required id='indexNumber' value={indexNumber}  onChange={(e) => setIndexNumber(e.target.value)} ></TextField>
                        </>
                    )}
                    <Label>Email:</Label>
                    <TextField required id='email' value={email} onChange={(e) => setEmail(e.target.value)} ></TextField>
                    <Label>Password:</Label>
                    <TextField required id='password' value={password} onChange={(e) => setPassword(e.target.value)} ></TextField>

                    <Button type='submit' variant='contained' onClick={handleUpdate} >Update Profile!</Button>
                </>
            )}
        </div>
    )
}

export default Profile