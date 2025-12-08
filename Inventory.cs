using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct item
{
    public int ItemID;
    public int ItemQuantity;
    [MarshalAs(unmanagedType:UnmanagedType.I1)]
    public bool ItemHasLimit;
    public int MaxItemQuantity;
    public int InItems;


}


public class Inventory : MonoBehaviour
{
    [DllImport("Inventory_2", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool AddItem(IntPtr Inventory, int inventorySize, int itemID, int quantity, bool hasLimit, int maxQuantity, int inItem);

    [DllImport("Inventory_2", CallingConvention = CallingConvention.Cdecl)]
    private static extern bool RemoveItem(IntPtr Inventory, int inventorySize, int itemID, int itemQuantity, int slots);

    [DllImport("Inventory_2", CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetItemQuantity(IntPtr Inventory, int inventorySize, int itemID);

    [DllImport("Inventory_2", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ClearSlot(IntPtr Inventory, int inventorySize, int slots);

    [SerializeField]
    public item[] inventory;
    //public List<item> inventory;
    private IntPtr unmanagedInventory;
    private const int INVENTORY_SIZE = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //print(inventory.Count<item>());

        
         SpawnInv();
         unmanagedInventory = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(item)) * INVENTORY_SIZE);
         CopyToUnmanaged();
        
        
        
            
            //CopyFromUnmanaged();
        

        /*
        try 
        {
            if (inventory != null)
            {
                return;
            }

        }
        catch (Exception Ex)
        {
            Debug.Log("fail"+Ex,this);
        }
        */

    }
    private void SpawnInv()
    {
        inventory = new item[INVENTORY_SIZE];


        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            inventory[i] = new item
            {
                ItemID = -1,
                ItemQuantity = 0,
                ItemHasLimit = false,
                MaxItemQuantity = 0,
                InItems = 0

            };
        }

    }

    private void CopyToUnmanaged()
    {
        IntPtr current = unmanagedInventory;
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            Marshal.StructureToPtr(inventory[i], current, false);
            current = (IntPtr)(current.ToInt64() + Marshal.SizeOf(typeof(item)));
        }
    }

    // copy unmanaged memory back to managed array, makes sure C++ can read it as its not rubbish collecetedd
    private void CopyFromUnmanaged()
    {
        IntPtr current = unmanagedInventory;
        for (int i = 0; i < INVENTORY_SIZE; i++)
        {
            inventory[i] = (item)Marshal.PtrToStructure(current, typeof(item));
            current = (IntPtr)(current.ToInt64() + Marshal.SizeOf(typeof(item)));
        }
    }
    public bool AddItemToInventory(int itemID, int itemQuantity, bool hasLimit, int maxQuantity, int inItems)
    {
        CopyToUnmanaged();

        bool result = AddItem(unmanagedInventory, INVENTORY_SIZE, itemID, itemQuantity, hasLimit, maxQuantity, inItems);
        if (result)
        {
            CopyFromUnmanaged();
        }
        return result;
    }

    public bool RemoveItemFromInventory(int slots, int itemQuantity, int itemID)
    {

        CopyToUnmanaged();
        bool result = RemoveItem(unmanagedInventory, INVENTORY_SIZE, itemID,
                                itemQuantity, slots);
        if (result)
        {
            CopyFromUnmanaged();
        }
        return result;
    }

    public int GetItemQuantityInInventory(int itemID)
    {
        CopyToUnmanaged();  // Ensure unmanaged memory is up to date
        // Last parameter is unused, so pass 0
        return GetItemQuantity(unmanagedInventory, INVENTORY_SIZE, itemID);
    }
    /*
    void OnDestroy()
    {
        if (unmanagedInventory != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(unmanagedInventory);
            unmanagedInventory = IntPtr.Zero;
        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
}
