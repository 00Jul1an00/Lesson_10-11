namespace Sample
{
    public static class InventoryExtension
    {
        public static void Clear(this Inventory inventory)
        {
            var items = inventory.GetItems();

            foreach (var item in items)
            {
                inventory.RemoveItem(item);
            }
        }
    }
}