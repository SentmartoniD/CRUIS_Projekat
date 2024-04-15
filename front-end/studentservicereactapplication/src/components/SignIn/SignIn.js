import { useEffect, useState, useContext } from 'react';
import { SigInProfessor } from '../../services/ProfessorService';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Box from '@mui/material/Box';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import { ThemeProvider } from '@mui/material/styles';
import { UserContext } from '../../contexts/UserContext';
import { useNavigate } from 'react-router-dom';
import { SigInStudent } from '../../services/StudentService';
import { Link } from "react-router-dom";
import './SignIn.css'
import { createTheme } from '@mui/material/styles';

// Create a theme instance
const theme = createTheme();

const SignIn = () => {
    const {setCurrentUser} = useContext(UserContext)

    const [indexNumber, setIndexNumber] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")

    const [isUserChecked, setIsUserChecked] = useState(0);

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
            if (!err?.response)
                alert("No server response, login failed!");
            else
                alert(JSON.stringify(err.response.data));
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
            if (!err?.response)
                alert("No server response, login failed!");
            else
                alert(JSON.stringify(err.response.data));
        }
    }

    const handleUserChange = (e) => {
        setIsUserChecked(parseInt(e.target.value));
        console.log(e.target.value)
    };

    return(
        <Box display="flex" flexDirection="column" alignItems="center" margin="200px auto" border={1} borderRadius={5} borderColor="primary.main" p={2} width={350} >
            <ThemeProvider theme={theme}>
                <Typography color="primary" variant='h5'>Sign in!</Typography>
            </ThemeProvider>
        
            <Select style={{ marginTop: '30px', marginBottom: '30px' }} onChange={(e) => handleUserChange(e)} value={isUserChecked} >
                <MenuItem value={0} >Select user type!</MenuItem>
                <MenuItem value={1} >Student</MenuItem>
                <MenuItem value={2} >Professor</MenuItem>
            </Select>

            {
                isUserChecked !== 0 ? isUserChecked === 1 ? (
                <Box  display="flex" flexDirection="column" alignItems="center" margin="20px">
                    <TextField style={{ marginBottom: '30px' }} required id='indexNumber' label="Index number" onChange={(e) => setIndexNumber(e.target.value)} ></TextField>
                    <TextField style={{ marginBottom: '30px' }} required id='passwordStudent' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>
                    <Button type='submit' variant='contained' onClick={handleLoginStudent} >Sign in!</Button>
                </Box> 
                )            
                :
                (
                <Box display="flex" flexDirection="column" alignItems="center" margin="20px">
                    <TextField style={{ marginBottom: '30px' }} required id='email' label="Email Address" onChange={(e) => setEmail(e.target.value)} ></TextField>
                    <TextField style={{ marginBottom: '30px' }} required id='passwordProfessor' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>
                    <Button  type='submit' variant='contained' onClick={handleLoginProfessor} >Sign in!</Button>
                </Box>
                ) 
                : null
            }
            <Link to={"/signup"} >Dont have an account? Sign up!</Link>
        </Box>
    )
}

export default SignIn