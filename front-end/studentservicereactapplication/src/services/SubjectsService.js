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

export const GetUnAthendedSubjects = async (idxnmb, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/subject/unathended/`+idxnmb, GetConfig(token))
}

export const RemoveAthendedSubject = async (id, idxnmb, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/subject/athended-remove/`+id+"/"+idxnmb, GetConfig(token))
}

export const AddUnAthendedSubject = async (id, idxnmb, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/subject/unathended-add/`+id+"/"+idxnmb, GetConfig(token))
}

export const GetSubjects = async (email, token) => {
    return await axios.get(`${process.env.REACT_APP_API_URL}/api/subject/professor-all/`+email, GetConfig(token))
}
