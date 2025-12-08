#pragma once

#ifdef _WIN32
#define API_EXPORT __declspec(dllexport)  
#else  
#define API_EXPORT  
#endif 


// this is the item that goes into the slot, the details that will be in that slot //
struct Item
{
	int ItemID;
	int ItemQuantity;
	bool ItemHasLimit;
	int MaxItemQuantity;
	int InItems;
};

extern "C"
{
	API_EXPORT bool AddItem(Item* Inventory, int inventorySize, int itemID, int itemQuantity, bool itemHasLimit, int maxItemQuantity, int inItems);

	// this one is more just a check, so check they have this amount then you call RemoveItem //
	API_EXPORT int GetItemQuantity(Item* Inventory, int inventorySize, int itemID);

	API_EXPORT bool RemoveItem(Item* Inventory, int inventorySize, int itemID, int itemQuantity, int slots);

	// this can clear itself if there are no items in the slot its just available for maunal clear //
	API_EXPORT void ClearSlot(Item* Inventory, int inventorySize, int Slots);
}