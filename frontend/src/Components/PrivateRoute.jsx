import React from 'react';
import { useNavigate } from 'react-router-dom';

const PrivateRoute = ({ element, isLoggedIn }) => {
    const navigate = useNavigate();

    return (
        isLoggedIn ? (
            element
        ) : (
            <div className='homeDiv'>
                <p>You're not supposed to be here!</p>
                <button onClick={()=>{navigate("/")}}>Go back</button>
            </div>
        )
    );
};

export default PrivateRoute;
