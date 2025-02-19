using System.Collections.Generic;

/// <summary>
/// Interface for managing the player's inventory system.
/// Handles item storage, retrieval, and weight-based constraints.
/// </summary>

public interface IInventoryService
{
    IEnumerable<InventorySlot> Slots { get; } // Collection of inventory slots, each representing an item stack in the inventory.
    void AddItem(ItemSO item, int quantity); // Adds a specified quantity of an item to the inventory.
    void RemoveItem(ItemSO item, int quantity); // Removes a specified quantity of an item from the inventory.
    bool CanAddItem(float weight); // Checks if an item can be added to the inventory without exceeding the weight limit.
    bool HasItem(ItemSO item, int quantity); // Checks if the inventory contains a specific item in the required quantity.
    int GetItemQuantity(ItemSO currentItem); // Retrieves the current quantity of a specific item in the inventory.

    float CurrentWeight { get; } // Gets the current total weight of items in the inventory.
    float MaxWeight { get; } // Gets the maximum weight capacity of the inventory.
    float TotalValue { get; } // Gets the total value of all items in the inventory, typically for trade or display purposes.

}