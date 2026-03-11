using System;
using System.Collections.Generic;

Main();

static void Main()
{
    Inventory inventory = new Inventory();
    InventoryUI ui = new InventoryUI();
    AutoBuyer autoBuyer = new AutoBuyer();

    inventory.ItemChanged += ui.Print;
    inventory.ItemChanged += autoBuyer.Print;

    inventory.AddItem("포션", 5);
    inventory.AddItem("화살", 10);
    inventory.AddItem("포션", 3);
    inventory.RemoveItem("화살", 7);
    inventory.RemoveItem("화살", 5);
}

class Inventory // 게시자
{
    Dictionary<string, int> inventory = new Dictionary<string, int> { };

    public event Action<string, int, int> ItemChanged;

    public void AddItem(string name, int count)
    {
        int oldCount;

        if(inventory.TryAdd(name, count))
        {
            oldCount = 0;
        }

        else
        {
            oldCount = inventory[name];
            inventory[name] += count;
        }

        ItemChanged?.Invoke(name, oldCount, inventory[name]);
    }

    public void RemoveItem(string name, int count)
    {
        int oldCount = inventory[name];
        inventory[name] -= count;
        if (inventory[name] < 0) inventory[name] = 0;
        ItemChanged?.Invoke(name, oldCount, inventory[name]);
    }
}

class InventoryUI   // 구독자1
{
    public void Print(string name, int oldCount, int newCount)
    {
        Console.WriteLine($"[UI] {name}: {oldCount} -> {newCount}");
    }
}

class AutoBuyer // 구독자2
{
    public void Print(string name, int oldCount, int newCount)
    {
        if (newCount == 0)
            Console.WriteLine($"[자동구매] {name} 재고 소진! 자동 구매 요청");
    }
}