
interface HeaderProps {
    
}

export function Header(props : HeaderProps)
{
    return (
        <header className="Header">
            <div className="container">
                <div className="header__logo"><a href="/">Anticafe</a></div>
                

            </div>
        </header>
    )
}

export default Header;