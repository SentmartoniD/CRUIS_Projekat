import { useEffect, useState } from "react";
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { GetSubjects } from '../../services/SubjectsService';

const Subjects = () => {
    const {currentUser} = useContext(UserContext);
    const [trigger, setTrigger] = useState(0)
    const [subjects, setSubjects] = useState([])

    const handleChange = async (subjectID) => {
        const [header, payload, signature] = currentUser.split('.');
        const decodedPayload = atob(payload);
        const payloadObj = JSON.parse(decodedPayload);
        const eml = payloadObj["email"]
        
        setTrigger(trigger + 1)
      };

    useEffect(() =>{
        const getSubjects = async () => {
            try
            {
                const [header, payload, signature] = currentUser.split('.');
                const decodedPayload = atob(payload);
                const payloadObj = JSON.parse(decodedPayload);
                const eml = payloadObj["email"]
                const response = await GetSubjects(eml, currentUser);
                console.log(response.data)
                setSubjects(response.data)
            }
            catch(err)
            {
                alert(err)
            }
        }
        getSubjects()
    }, [trigger])

    return(
        <div>
            <Typography variant='h5' >My subjects!</Typography>
            <div>
                {subjects.length === 0 ? <h5>No subjects!</h5> : (
                    <ul>
                        {subjects.map((subject) => (
                            <li id={subject.id} >
                                <label>Naziv : {subject.name}</label>
                                <label>Godina odrzavanja : {subject.year}</label>
                                <Button variant='contained' onClick={() => handleChange(subject.id)} >Prijavi se!</Button>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    )
}

export default Subjects