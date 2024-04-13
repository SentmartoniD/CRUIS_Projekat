import { useState } from 'react';
import Typography from '@mui/material/Typography';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import { Link } from 'react-router-dom';
import { SigUpStudent } from '../../services/StudentService';

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
            alert(err)
        }
    }


    return (
        <div>
            <Typography variant='h5' >Sign up!</Typography>

            <TextField required id='firstName' label="First name" onChange={(e) => setFirstName(e.target.value)} ></TextField>
            <TextField required id='lastName' label="Last name" onChange={(e) => setLastName(e.target.value)} ></TextField>
            <TextField required id='indexNumber' label="Index number" onChange={(e) => setIndexNumber(e.target.value)} ></TextField>
            <TextField required id='email' label="Email" onChange={(e) => setEmail(e.target.value)} ></TextField>
            <TextField required id='password' label="Password" onChange={(e) => setPassword(e.target.value)} ></TextField>

            <Button type='submit' variant='contained' onClick={handleSignUp} >Sign in!</Button>

            <Link to={"/signin"} >Already have an account? Sign in!</Link>
        </div>
    )
}




export default SignUp