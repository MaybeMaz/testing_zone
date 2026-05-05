using Sandbox;

public class InventoryItem
{
	public string Name { get; set; }
	public int Amount { get; set; }

	public InventoryItem( string name, int amount )
	{
		Name = name;
		Amount = amount;
	}
}