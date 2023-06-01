import React from "react";
import { Inventory } from "../models/inventory";

interface InventoryProps {
    inventory: Inventory
}

export function InventoryInfo({ inventory }: InventoryProps)
{
    return (
        <div className="item">
            <p>{inventory.name}</p>
        </div>
    )
}

export function InventoryPage() {
    
}

export default InventoryInfo;