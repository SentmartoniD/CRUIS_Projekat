import axios from "axios"

export const GetConfig = (token) => {
    return {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    }
}

export const SigInStudent = async (ind, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/student/signin`, {indexnumber: ind, password: pwd})
}

export const SigUpStudent = async (fn, ln, ind, eml, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/student/signup`, {firstname: fn, lastname: ln, indexnumber: ind, email: eml, password: pwd})
}

export const GetStudent = async (idxnmb, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/student/by-indexNumber/`+idxnmb, GetConfig(token))
}

export const UpdateStudent = async (id, fn, ln, ind, eml, pwd, token) => {
    return await axios.put(`${process.env.REACT_APP_API_URL}/api/student/update`, 
    { id: id, firstname: fn, lastname: ln, indexnumber: ind, email: eml, password: pwd}, 
    GetConfig(token))
}