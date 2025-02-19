/// <summary>
/// Represents a slot in the inventory that holds a specific item and its quantity.
/// This class is used to manage items within the player's inventory.
/// </summary>

public class InventorySlot
{
    public ItemSO Item { get; set; } // The item stored in this inventory slot.
    public int Quantity { get; set; } // The quantity of the item present in this slot.
}