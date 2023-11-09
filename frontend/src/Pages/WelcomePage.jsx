import { useNavigate } from "react-router-dom";

const WelcomePage = () => {
    const navigate = useNavigate();
    
    return (
        <div>
            <button onClick={()=>{navigate("/registration")}}>Register</button>
            <button onClick={()=>{navigate("/login")}}>Login</button>
        </div>
    );
}

export default WelcomePage;