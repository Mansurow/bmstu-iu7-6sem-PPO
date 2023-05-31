import React, { useState } from 'react';
import "../common/forms/Forms.css";
import { useActions } from '../../hooks/useActions';
import { Gender } from '../../models/enums/gender.enum';
import { User } from '../../models/user';
import { UserType } from '../../models/enums/usertype.enum';
import { useNavigate } from 'react-router-dom';

interface SignUp {}

const SignUp: React.FC<SignUp> = () => {
    const [name, setName] = useState('');
    const [firstName, setFirstName] = useState('');
    const [surname, setSurname] = useState('');
    const [phone, setPhone] = useState('');
    const [gender, setGender] = useState<Gender>(Gender.Unknown);
    const [birthday, setBirthday] = useState('');
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');

    const dispatch  = useActions();
    const navigate = useNavigate();

    const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        console.log({
            surname,
            name,
            firstName,
            gender,
            birthday,
            phone,
            login,
            password,
        })
        
        const newUser: User = {
            id: 10,
            name: name,
            surname: surname,
            firstname: firstName,
            gender: gender,
            birthday: birthday,
            email: login,
            password: password,
            phone: phone,
            role: UserType.User 
        };

        dispatch.createUser(newUser);
        navigate("/")
    };

    return (
        <form onSubmit={handleSubmit} className="form">
            <label>
                Имя:
                <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
            </label>
            <label>
                Фамилия:
                <input type="text" value={surname} onChange={(e) => setSurname(e.target.value)} />
            </label>
            <label>
                Отчество:
                <input type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
            </label>
            <label>
                Пол:
                <select value={gender} onChange={(e) => setGender(e.target.value as Gender)}>
                    <option value={Gender.Male}>Мужской</option>
                    <option value={Gender.Female}>Женский</option>
                </select>
            </label>
            <label>
                Дата рождения:
                <input type="text" value={birthday} onChange={(e) => setBirthday(e.target.value)} />
            </label>
            <label>
                Телефон:
                <input type="text" value={phone} onChange={(e) => setPhone(e.target.value)} />
            </label>
            <label>
                Логин (Email):
                <input type="text" value={login} onChange={(e) => setLogin(e.target.value)} />
            </label>
            <label>
                Пароль:
                <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} />
            </label>
            <button type="submit">Зарегистрироваться</button>
        </form>
    );
};

export default SignUp;
