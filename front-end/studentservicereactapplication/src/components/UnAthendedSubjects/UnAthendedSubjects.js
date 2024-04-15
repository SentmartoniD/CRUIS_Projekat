import { useEffect, useState } from "react";
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { GetUnAthendedSubjects, AddUnAthendedSubject } from '../../services/SubjectsService';

const UnAthendedSubjects = () => {
    const {currentUser} = useContext(UserContext);
    const [trigger, setTrigger] = useState(0)
    const [subjects, setSubjects] = useState([])

    const handleChange = async (subjectID) => {
        const [header, payload, signature] = currentUser.split('.');
        const decodedPayload = atob(payload);
        const payloadObj = JSON.parse(decodedPayload);
        const idxnmb = payloadObj["indexNumber"]
        try{
            const response = await AddUnAthendedSubject(subjectID, idxnmb, currentUser)
            alert("Add subject successful!")
        }
        catch(err)
        {
            if (!err?.response)
                alert("No server response, login failed!");
            else
                alert(JSON.stringify(err.response.data));
        }
        setTrigger(trigger + 1)
      };

    useEffect(() =>{
        const getSubjects = async () => {
            try
            {
                const [header, payload, signature] = currentUser.split('.');
                const decodedPayload = atob(payload);
                const payloadObj = JSON.parse(decodedPayload);
                const idxnmb = payloadObj["indexNumber"]
                const response = await GetUnAthendedSubjects(idxnmb, currentUser);
                console.log(response.data)
                setSubjects(response.data)
            }
            catch(err)
            {
                if (!err?.response)
                    alert("No server response, login failed!");
                else
                    alert(JSON.stringify(err.response.data));
            }
        }
        getSubjects()
    }, [trigger])

    return(
        <div>
            <Typography variant='h5' >All subjects!</Typography>
            <div>
                {subjects.length === 0 ? <h5>No subjects!</h5> : (
                    <ul>
                        {subjects.map((subject) => (
                            <li id={subject.id} >
                                <label>Naziv : {subject.name}</label>
                                <label>Godina odrzavanja : {subject.year}</label>
                                <label>Professor : {subject.professor.firstName + " " + subject.professor.lastName}</label>
                                <label>Email : {subject.professor.email}</label>
                                <Button variant='contained' onClick={() => handleChange(subject.id)} >Prijavi se!</Button>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    )
}

export default UnAthendedSubjects