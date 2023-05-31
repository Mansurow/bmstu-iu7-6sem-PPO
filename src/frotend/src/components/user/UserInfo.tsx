import React, { useState } from 'react';
import { useSelector} from 'react-redux';
import {RootState} from "../../redux/store/store";
import {useActions} from "../../hooks/useActions";
import { User } from '../../models/user';



const UserInfo: React.FC = () => {
    const user = useSelector<RootState, User>((state) => state.currentUser);
    const [isEditing, setIsEditing] = useState(false);

    const dispatch = useActions();

    const handleEditClick = () => {
        setIsEditing(true);
    };

    return (
        <div className="userInfo">
            <h2>Информация о пользователе</h2>
            <p>
                <strong>ФИО:</strong> {user.surname} {user.name} {user.firstname}
            </p>
            <p>
                <strong>Пол:</strong> {user.gender}
            </p>
            <p>
                <strong>Телефон:</strong> {user.phone}
            </p>
            <p>
                <strong>Дата рождения:</strong> {user.birthday}
            </p>
            <p>
                <strong>Email:</strong> {user.email}
            </p>
        </div>
    );
};

export default UserInfo;