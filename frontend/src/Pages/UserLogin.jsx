import {useState} from "react";

const UserLogin = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const onSubmit = (e) =>{
        e.PreventDefault();
    }

    return (
        <form onSubmit={onSubmit}>
            <div>
                <label htmlFor="email">Email: </label>
                <input
                name="email"
                onChange={(e)=> {setEmail(e.target.value)}}
                id="email"
                value={email}
                />
            </div>
            
            <div>
                <label htmlFor="password">password: </label>
                <input
                name="password"
                onChange={(e)=> {setPassword(e.target.value)}}
                id="password"
                value={email}
                />
            </div>
        </form>
    );
}

export default UserLogin;