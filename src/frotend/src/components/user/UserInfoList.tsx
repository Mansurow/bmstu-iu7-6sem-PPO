import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { useActions } from '../../hooks/useActions';
import { RootState } from '../../redux/store/store';
import { User } from '../../models/user';
import './UserInfoList.css';

const UserInfoList: React.FC = () => {
    const dispatch = useActions();
    const history = useNavigate();
    const userInfo = useSelector<RootState, User[]>(
        (state) => state.user.users
    );

    useEffect(() => {
        dispatch.getAllUsers();
    }, [dispatch]);

    const handleUserClick = (userId: number) => {
        history(`/user/${userId}`);
    };

    const handleHonorClick = (userId: number) => {
        console.log(`Honor clicked for user ${userId}`);
    };

    // if (loading) {
    //     return <div>Loading...</div>;
    // }

    // if (error) {
    //     return <div>Error: {error}</div>;
    // }

    return (
        <div>
            {userInfo.map((user) => (
                <div key={user.id} className="user-card">
                    <h2>{`${user.surname} ${user.name} ${user.firstname}`}</h2>
                    <p>Телефон: {user.phone || 'N/A'}</p>
                    <p>Пол: {user.gender}</p>
                    <p>Возраст: {user.birthday}</p>
                    <p>Email: {user.email}</p>
                </div>
            ))}
        </div>
    );
};

export default UserInfoList;
