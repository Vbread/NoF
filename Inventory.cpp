#include "pch.h"
#include "Inventory.h"

bool AddItem(Item* Inventory, int inventorySize, int itemID, int itemQuantity, bool itemHasLimit, int maxItemQuantity, int inItems)
{
	// checking if the thing calling has an inventory or is trying to add nothing or take away adding //
	if (!Inventory || itemQuantity <= 0)
	{
		return false;
	}

	for (int i = 0; i < inventorySize; i++) // check if it can stack with something //
	{

		if (Inventory[i].ItemID == itemID) 
		{

			if (Inventory[i].ItemHasLimit == false) // if the item doesnt have a limit //
			{

				Inventory[i].ItemQuantity += inItems;
				return true;

			}
			else
			{

				int SpaceLeft = maxItemQuantity - Inventory[i].ItemQuantity;
				if (SpaceLeft >= inItems)
				{

					Inventory[i].ItemQuantity += inItems;
					return true;
				}
				// contines if items the stack is full
			}

		}

		

		
	}
	// find item slot //
	for (int i = 0; i < inventorySize; i++)
	{

		if (Inventory[i].ItemID == -1)
		{
			// set paramaters for new item ID
			Inventory[i].ItemID = itemID;
			Inventory[i].ItemQuantity = itemQuantity;
			Inventory[i].ItemHasLimit = itemHasLimit;
			Inventory[i].MaxItemQuantity = maxItemQuantity;
			return true;

		}

	}

	return false; // no space in inventory :(

}


int GetItemQuantity(Item* Inventory, int inventorySize, int itemID)
{

	if (!Inventory)
	{
		return 0;
	}

	int total = 0;

	// returns the amound of items under ItemID //
	for (int i = 0; i < inventorySize; i++)
	{

		if (Inventory[i].ItemID == itemID)
		{

			total += Inventory[i].ItemQuantity;

		}

	}
	return total;

}

bool RemoveItem(Item* Inventory, int inventorySize, int itemID, int itemQuantity, int Slots)
{
	// similar to additem, check to see if what is calling is a valid call //
	if (!Inventory || itemQuantity <= 0 || Slots < 0 || Slots >= inventorySize)
	{
		return false;
	}

	Item& item = Inventory[Slots];

	// check if called item is the same as item in slot. check if item called is allowed to remove that much (cant remove 10 from 1) //
	if (item.ItemID != itemID || item.ItemQuantity < itemQuantity)
	{
		return false;
	}

	item.ItemQuantity -= itemQuantity;

	if (item.ItemQuantity <= 0)
	{
		ClearSlot(Inventory, inventorySize, Slots);
	}

	return true;
}

void ClearSlot(Item* Inventory, int inventorySize, int Slots)
{
	// make slot available again //
	if (Inventory && Slots >= 0)
	{

		Inventory[Slots].ItemID = -1;
		Inventory[Slots].ItemHasLimit = false;
		Inventory[Slots].ItemQuantity = 0;
		Inventory[Slots].MaxItemQuantity = 0;
		Inventory[Slots].InItems = 0;
	}

}