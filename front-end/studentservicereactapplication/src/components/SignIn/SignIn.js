import { useEffect, useState, useContext } from 'react';
import { SigInProfessor } from '../../services/ProfessorService';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { UserContext } from '../../contexts/UserContext';
import { useNavigate } from 'react-router-dom';


const SignIn = () => {
    const {setCurrentUser} = useContext(UserContext)

    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")

    const navigate = useNavigate()
    const navigateToHome = () => {
        navigate("/")
    }
    
    const handleLogin = async () => {
        try{
            const response = await SigInProfessor(email, password);
            const token = response.data
            setCurrentUser(token)    
            navigateToHome()
        }
        catch(err){
            alert(err)
        }

    }

    return(
        <div>
            <Typography variant='h5'>Sign in!</Typography>
            <TextField required id='email' label="Email Address" onChange={(e) => setEmail(e.target.value)} ></TextField>
            <TextField required id='password' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>
            <Button type='submit' variant='contained' onClick={handleLogin} >Sign in!</Button>
        </div>
    )
}

export default SignIn