import { useState } from "react";
import { useActions } from "../../hooks/useActions";
import { Room } from "../../models/room";

const ClubForm: React.FC = () => {;    
    const [editName, setEditName] = useState('');
    const [editPrice, setEditPrice] = useState(0);
    const [editSize, setEditSize] = useState(0);    
    const [editInventories, setEditInventories] = useState([]);
        
    const dispatch = useActions();

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        const newClub: Room = {
            id: 0,
            name: editName,
            price: editPrice,
            size: editSize,
            rating: 0,
            inventories: editInventories
        };

        dispatch.createRoom(newClub);
        // Clear form fields
        setEditName('');
        setEditPrice(0);
        setEditSize(0);
        setEditInventories([]);
    };

    return (
        <form onSubmit={handleSubmit} className="form">
            <h2>Create Club</h2>
            <div>
                <label htmlFor="name">Название:</label>
                <input
                    type="text"
                    id="name"
                    value={editName}
                    onChange={(e) => setEditName(e.target.value)}
                    required
                />
            </div>
            <div>
                <label htmlFor="price">Цена:</label>
                <input
                    type="number"
                    id="price"
                    value={editPrice}
                    onChange={(e) => setEditPrice(Number(e.target.value))}
                    required
                />
            </div>
            <div>
                <label htmlFor="size">Площадь:</label>
                <input
                    type="number"
                    id="size"
                    value={editSize}
                    onChange={(e) => setEditSize(Number(e.target.value))}
                    required
                />
            </div>
            <button type="submit">Создать комнату</button>
        </form>
    );
}

export default ClubForm;
