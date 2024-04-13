import axios from "axios"

export const SigInProfessor = async (eml, pwd) => {
    return await axios.post(`${process.env.REACT_APP_API_URL}/api/professor/signin`, {email: eml, password: pwd})
}