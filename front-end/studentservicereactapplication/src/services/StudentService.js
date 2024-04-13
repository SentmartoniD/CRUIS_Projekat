import axios from "axios"

export const SigInStudent = async (ind, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/student/signin`, {indexnumber: ind, password: pwd})
}