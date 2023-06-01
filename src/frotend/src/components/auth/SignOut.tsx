import React, { useEffect } from 'react';
import "../common/forms/Forms.css";
import { useActions } from '../../hooks/useActions';
import { useNavigate } from 'react-router-dom';

interface SignOutProps {
    // Add any props you need for the SignOut component
}

const SignOut: React.FC<SignOutProps> = () => {
    const dispatch = useActions();
    const navigate = useNavigate();

    const handleYesSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        dispatch.signOut()
        navigate('/')
    };

    const handleNoSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        navigate('/')
    };

    // Add sign out logic here, such as clearing authentication tokens or session data
    return (
        <div className="form">
            <h2>Вы уверены, что хотите выйти?</h2>
            <form className="sign-in-form">
                <button type="submit" onClick={handleYesSubmit}>Да</button>
                <button type="submit" onClick={handleNoSubmit}>Нет</button>
            </form>
        </div>
    );
};

export default SignOut;
