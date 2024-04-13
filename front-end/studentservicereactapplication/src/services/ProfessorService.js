import axios from "axios"

export const GetConfig = (token) => {
    return {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    }
}

export const SigInProfessor = async (eml, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/professor/signin`, {email: eml, password: pwd})
}

export const GetProfesor = async (eml, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/professor/by-email/`+eml, GetConfig(token))
}

export const UpdateProfessor = async (id, fn, ln, eml, pwd, token) => {
    return await axios.put(`${process.env.REACT_APP_API_URL}/api/professor/update`, 
    { id: id, firstname: fn, lastname: ln, email: eml, password: pwd}, 
    GetConfig(token))
}