import { useEffect, useState, useContext } from 'react';
import { SigInProfessor } from '../../services/ProfessorService';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { UserContext } from '../../contexts/UserContext';
import { useNavigate } from 'react-router-dom';
import { SigInStudent } from '../../services/StudentService';
import { Link } from "react-router-dom";


const SignIn = () => {
    const {setCurrentUser} = useContext(UserContext)

    const [indexNumber, setIndexNumber] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")

    const navigate = useNavigate()
    const navigateToHome = () => {
        navigate("/")
    }
    
    const handleLoginStudent = async () => {
        try{
            const response = await SigInStudent(indexNumber, password);
            const token = response.data
            setCurrentUser(token)    
            navigateToHome()
        }
        catch(err){
            alert(err)
        }
    }

    const handleLoginProfessor = async () => {
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
            <TextField required id='indexNumber' label="Index number" onChange={(e) => setIndexNumber(e.target.value)} ></TextField>
            <TextField required id='passwordStudent' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>
            <Button type='submit' variant='contained' onClick={handleLoginStudent} >Sign instudent!</Button>

            <TextField required id='email' label="Email Address" onChange={(e) => setEmail(e.target.value)} ></TextField>
            <TextField required id='passwordProfessor' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>
            <Button type='submit' variant='contained' onClick={handleLoginProfessor} >Sign in!</Button>

            <Link to={"/signup"} >Dont have an account? Sign up!</Link>
        </div>
    )
}

export default SignIn