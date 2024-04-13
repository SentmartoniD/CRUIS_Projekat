import axios from "axios"

export const SigInStudent = async (ind, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/student/signin`, {indexnumber: ind, password: pwd})
}

export const SigUpStudent = async (fn, ln, ind, eml, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/student/signup`, {firstname: fn, lastname: ln, indexnumber: ind, email: eml, password: pwd})
}