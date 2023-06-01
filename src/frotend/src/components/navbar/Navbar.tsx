import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';
import { UserType } from '../../models/enums/usertype.enum';
import { LoginProps } from '../../models/types';

const Navbar: React.FC<LoginProps> = ({role,isLogin}) => {
    console.log(role)
    return (
        <div className="navbar-container">
            <header>
                <nav className="menu">
                    <div className="menu-items">
                        <ul>
                            <li className="menu-item"><Link to="/">Anticafe</Link></li>
                            <li className="menu-item"><Link to="/rooms">Rooms</Link></li>
                            <li className="menu-item"><Link to="/menu">Menu</Link></li>
                            {
                                role === UserType.Admin && 
                                <>
                                    <li className="menu-item"><Link to="/users">Users</Link></li>
                                    <li className="menu-item"><Link to="/bookings">Bookings</Link></li>
                                </>
                            }
                            {
                                role === UserType.User && 
                                <>
                                    <li className="menu-item"><Link to="/bookings">Bookings</Link></li>
                                </>
                            }
                            {isLogin ?
                            <>
                                <li className="menu-item"><Link to="/users/me">Profile</Link></li>
                                <li className="menu-item"><Link to="/signOut">Sign Out</Link></li>
                            </>:
                                <>
                                <li className="menu-item"><Link to="/signIn">Sign In</Link></li>
                                <li className="menu-item"><Link to="/signUp">Sign Up</Link></li>
                                </>
                            }
                        </ul>
                    </div>
                </nav>
            </header>
            <main>
                {/* Add the main content of your home page here */}
            </main>
        </div>
    );
};

export default Navbar;
