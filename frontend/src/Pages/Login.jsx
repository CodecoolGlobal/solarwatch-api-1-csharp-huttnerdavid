import { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Login = ({setLogin}) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        const user = {Email: email, Password: password};

        try{
            const response = await axios.post("http://localhost:5279/Auth/Login", user);
            if(response.status === 200){
                navigate("/");
                setLogin(true);
                axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                localStorage.setItem("loggedIn", "true")
            }
        }
        catch(error){
            alert("Registration unsuccesfull!");
        }
    }

    return(
    <div className="formTemplate">
        <p>Login</p>
        <input type="text" placeholder="email" onChange={(e)=>{setEmail(e.target.value)}}/><br/>
        <input type="password" placeholder="password" onChange={(e)=>{setPassword(e.target.value)}}/><br/>
        <button onClick={handleSubmit}>Submit</button>
    </div>)
}
export default Login;