import React, { useState } from 'react';
import "../common/forms/Forms.css";
import {useActions} from "../../hooks/useActions";
import {useNavigate} from "react-router-dom";

interface SignInProps {
    // Add any props you need for the SignIn component
}

const SignIn: React.FC<SignInProps> = () => {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const dispatch = useActions();
    const navigate = useNavigate();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        dispatch.signIn({login, password})
        navigate("/")
    };

    return (
        <div className="form">
            <h2>Sign In</h2>
            <form onSubmit={handleSubmit} className="sign-in-form">
                <div>
                    <label htmlFor="login">Login:</label>
                    <input
                        type="text"
                        id="login"
                        value={login}
                        onChange={(e) => setLogin(e.target.value)}
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit">Sign In</button>
            </form>
        </div>
    );
};

export default SignIn;
