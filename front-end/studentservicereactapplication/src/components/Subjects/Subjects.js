import { useEffect, useState } from "react";
import { useContext } from "react";
import { UserContext } from '../../contexts/UserContext'
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { GetSubjects } from '../../services/SubjectsService';

const Subjects = () => {
    const {currentUser} = useContext(UserContext);
    const [trigger, setTrigger] = useState(0);
    const [subjects, setSubjects] = useState([]);
    const [selectedData, setSelectedData] = useState({}); // State to store selected student and grade for each subject

    const handleChange = async (subjectID) => {
        const { selectedStudent, selectedGrade } = selectedData[subjectID] || {};
        if (selectedStudent && selectedGrade) {
            console.log(selectedStudent)
            console.log(selectedGrade)
            console.log(subjectID)
        } else {
            alert("Select a grade!");
        }
        setTrigger(trigger + 1);
    };

    useEffect(() => {
        const getSubjects = async () => {
            try {
                const [header, payload, signature] = currentUser.split('.');
                const decodedPayload = atob(payload);
                const payloadObj = JSON.parse(decodedPayload);
                const eml = payloadObj["email"];
                const response = await GetSubjects(eml, currentUser);
                console.log(response.data);
                setSubjects(response.data);
            } catch(err) {
                alert(err);
            }
        };
        getSubjects();
    }, [trigger]);

    const handleStudentChange = (event, subjectID) => {
        const newSelectedData = { ...selectedData };
        newSelectedData[subjectID] = { ...newSelectedData[subjectID], selectedStudent: event.target.value };
        setSelectedData(newSelectedData);
    };

    const handleGradeChange = (event, subjectID) => {
        const newSelectedData = { ...selectedData };
        newSelectedData[subjectID] = { ...newSelectedData[subjectID], selectedGrade: event.target.value };
        setSelectedData(newSelectedData);
    };

    return(
        <div>
            <Typography variant='h5'>My subjects!</Typography>
            {subjects.length === 0 ? <h5>No subjects!</h5> : (
                subjects.map((subject) => (
                    <div key={subject.id}>
                        <ul>
                            <li>
                                <label>Name: {subject.name}</label>
                                <label>Year of subject: {subject.year}</label>
                                <label>Students:</label>
                                <select onChange={(e) => handleStudentChange(e, subject.id)}>
                                    <option value={0}>--Select student--</option>
                                    {
                                        subject.studentGrades.map((grade, index) => (
                                            grade === 5 && subject.students[index] ?                                  
                                                <option key={subject.students[index].id} value={subject.students[index].id}>
                                                    {subject.students[index].firstName} {subject.students[index].lastName}, {subject.students[index].indexNumber}
                                                </option>
                                            : null
                                        ))
                                    }
                                </select>
                                {
                                    selectedData[subject.id]?.selectedStudent ? 
                                    <div>
                                        <select onChange={(e) => handleGradeChange(e, subject.id)}>
                                            <option value={5}>--Select grade--</option>
                                            <option value={6}>6</option>
                                            <option value={7}>7</option>
                                            <option value={8}>8</option>
                                            <option value={9}>9</option>
                                            <option value={10}>10</option>
                                        </select>
                                        <Button variant='contained' onClick={() => handleChange(subject.id)}>Change grade!</Button>
                                    </div>
                                    : null
                                }
                            </li>
                        </ul>
                        <table>
                            <thead>
                                <tr>
                                    <th>First name</th>
                                    <th>Last name</th>
                                    <th>Index number</th>
                                    <th>Grade</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    subject.studentGrades.map((grade, index) => (
                                        grade !== 5 && subject.students[index] ? 
                                            <tr key={subject.students[index].id}>
                                                <td>{subject.students[index].firstName}</td>
                                                <td>{subject.students[index].lastName}</td>
                                                <td>{subject.students[index].indexNumber}</td>
                                                <td>{grade}</td>
                                            </tr>    
                                        : null
                                    ))
                                }
                            </tbody>
                        </table>
                    </div>
                ))
            )}
        </div>
    );
};

export default Subjects