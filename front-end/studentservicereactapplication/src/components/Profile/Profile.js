import { useState, useEffect } from "react"
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Label from '@mui/material/Button';
import Box from '@mui/material/Box';
import { GetStudent } from "../../services/StudentService";
import { GetProfesor, UpdateProfessor } from "../../services/ProfessorService";
import { UpdateStudent } from "../../services/StudentService";
import { ThemeProvider } from '@mui/material/styles';
import { createTheme } from '@mui/material/styles';

// Create a theme instance
const theme = createTheme();

const Profile = () => {
    const {currentUser} = useContext(UserContext);
    const [trigger, setTrigger] = useState(0);
    const [firstName, setFirstName] = useState("")
    const [lastName, setLastName] = useState("")
    const [indexNumber, setIndexNumber] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")
    const [role, setRole] = useState(null);
    const [user, setUser] = useState(null);

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
                <Box display="flex" flexDirection="column" alignItems="center" margin="200px auto" border={1} borderRadius={5} borderColor="primary.main" p={2} width={350} height={600} >
                    <ThemeProvider theme={theme}>
                        <Typography color="primary" variant='h5'>My Profile!</Typography>
                    </ThemeProvider>
                    
                    <Box style={{ marginTop: '20px', marginBottom: '20px' }} Box display="flex" alignItems="center" gap={2} >
                        <Label>First name:</Label>
                        <TextField required id='firstName' value={firstName} onChange={(e) => setFirstName(e.target.value)} ></TextField>
                    </Box>
                    <Box style={{ marginTop: '20px', marginBottom: '20px' }} Box display="flex" alignItems="center" gap={2} >
                        <Label>Last name:</Label>
                        <TextField required id='lastName' value={lastName}  onChange={(e) => setLastName(e.target.value)} ></TextField>
                    </Box>
                    {indexNumber && (
                        <Box style={{ marginTop: '20px', marginBottom: '20px' }} Box display="flex" alignItems="center" gap={2} >
                            <Label>Index number:</Label>
                            <TextField style={{ marginRight: '35px' }} required id='indexNumber' value={indexNumber}  onChange={(e) => setIndexNumber(e.target.value)} ></TextField>
                        </Box>
                    )}
                    <Box style={{ marginTop: '20px', marginBottom: '20px' }} Box display="flex" alignItems="center" gap={2} >
                        <Label>Email:</Label>
                        <TextField style={{ marginLeft: '35px' }} required id='email' value={email} onChange={(e) => setEmail(e.target.value)} ></TextField>
                    </Box>
                    <Box style={{ marginTop: '20px', marginBottom: '20px' }} Box display="flex" alignItems="center" gap={2} >
                        <Label>Password:</Label>
                        <TextField required id='password' value={password} onChange={(e) => setPassword(e.target.value)} ></TextField>
                    </Box>
                    <Button style={{ marginTop: '20px' }} type='submit' variant='contained' onClick={handleUpdate} >Update Profile!</Button>
                </Box>
            )}
        </div>
    )
}

export default Profile