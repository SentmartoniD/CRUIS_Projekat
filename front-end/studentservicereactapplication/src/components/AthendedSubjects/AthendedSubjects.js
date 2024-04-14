import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useEffect, useState } from 'react';
import { GetAthendedSubjects, RemoveAthendedSubject } from '../../services/SubjectsService';

const AthendedSubjects= () => {
    const {currentUser} = useContext(UserContext);
    const [trigger, setTrigger] = useState(0)
    const [subjects, setSubjects] = useState([])

    const handleChange = async (subjectID) => {
        const [header, payload, signature] = currentUser.split('.');
        const decodedPayload = atob(payload);
        const payloadObj = JSON.parse(decodedPayload);
        const idxnmb = payloadObj["indexNumber"]
        const response = await RemoveAthendedSubject(subjectID, idxnmb, currentUser)
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
                const response = await GetAthendedSubjects(idxnmb, currentUser);
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

    return (
        <div>
            <Typography variant='h5' >My subjects!</Typography>
            <div>
                {subjects.length === 0 ? <h5>No subjects!</h5> : (
                    <ul>
                        {subjects.map((subject) => (
                            <li id={subject.id} >
                                <label>Naziv : {subject.name}</label>
                                <label>Godina odrzavanja : {subject.year}</label>
                                <label>Professor : {subject.professor.firstName + " " + subject.professor.lastName}</label>
                                <label>Email : {subject.professor.email}</label>
                                <label>Ocena : {subject.grade}</label>
                                <label>Polozen : {subject.grade === 5 ? "Ne" : "Da"}</label>
                                <Button variant='contained' onClick={() => handleChange(subject.id)} >Odustani</Button>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    )
}

export default AthendedSubjects