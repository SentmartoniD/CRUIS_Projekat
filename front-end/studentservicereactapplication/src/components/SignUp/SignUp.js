import { useState } from 'react';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { Link } from 'react-router-dom';
import { SigUpStudent } from '../../services/StudentService';
import { useNavigate } from 'react-router-dom';
import Box from '@mui/material/Box';
import { ThemeProvider } from '@mui/material/styles';
import { createTheme } from '@mui/material/styles';

// Create a theme instance
const theme = createTheme();

const SignUp = () => {

    const [firstName, setFirstName] = useState("")
    const [lastname, setLastName] = useState("")
    const [indexNumber, setIndexNumber] = useState("")
    const [email, setEmail] = useState("")
    const [password, setPassword] = useState("")

    const navigate = useNavigate()
    const navigateToSignIn = () => {
        navigate("/signin")
    }

    const handleSignUp = async () => {
        try{
            const response = await SigUpStudent(firstName, lastname, indexNumber, email, password);
            navigateToSignIn()
        }
        catch(err)
        {
            if (!err?.response)
                alert("No server response, login failed!");
            else
                alert(JSON.stringify(err.response.data));
        }
    }


    return (
        <Box display="flex" flexDirection="column" alignItems="center" margin="200px auto" border={1} borderRadius={5} borderColor="primary.main" p={2} width={350} >
            <ThemeProvider theme={theme}>
                <Typography color="primary" variant='h5'>Sign up!</Typography>
            </ThemeProvider>

            <TextField style={{ marginBottom: '20px', marginTop: '20px' }}  required id='firstName' label="First name" onChange={(e) => setFirstName(e.target.value)} ></TextField>
            <TextField style={{ marginBottom: '20px', marginTop: '20px' }} required id='lastName' label="Last name" onChange={(e) => setLastName(e.target.value)} ></TextField>
            <TextField style={{ marginBottom: '20px', marginTop: '20px' }} required id='indexNumber' label="Index number" onChange={(e) => setIndexNumber(e.target.value)} ></TextField>
            <TextField style={{ marginBottom: '20px', marginTop: '20px' }} required id='email' label="Email" onChange={(e) => setEmail(e.target.value)} ></TextField>
            <TextField style={{ marginBottom: '20px', marginTop: '20px' }} required id='password' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>

            <Button style={{ marginBottom: '10px', marginTop: '10px' }}  type='submit' variant='contained' onClick={handleSignUp} >Sign in!</Button>

            <Link to={"/signin"} >Already have an account? Sign in!</Link>
        </Box>
    )
}




export default SignUp