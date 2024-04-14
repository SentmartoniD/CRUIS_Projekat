import axios from "axios"

export const GetConfig = (token) => {
    return {
        headers: {
            "Authorization": `Bearer ${token}`
        }
    }
}

export const GetAthendedSubjects = async (idxnmb, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/subject/athended/`+idxnmb, GetConfig(token))
}