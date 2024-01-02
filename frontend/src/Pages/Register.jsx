import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Register = () => {
    const [email, setEmail] = useState("");
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        const user = {Email: email, Username: userName, Password: password};

        try{
            const response = await axios.post("http://localhost:5279/Auth/Register", user);
            if(response.status === 201){
                navigate("/");
                alert("Succesfully registered!");
            }
        }
        catch(error){
            alert("Registration unsuccesfull!");
        }
    }

    return(
    <div>
        <input type="text" placeholder="email" onChange={(e)=>{setEmail(e.target.value)}}/>
        <input type="text" placeholder="username" onChange={(e)=>{setUserName(e.target.value)}}/>
        <input type="password" placeholder="password" onChange={(e)=>{setPassword(e.target.value)}}/>
        <input type="button" onClick={handleSubmit} value={"Submit"}/>
    </div>
    )
}
export default Register;