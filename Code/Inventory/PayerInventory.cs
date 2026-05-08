using Sandbox;
using System.Collections.Generic;

public sealed class PlayerInventory : Component
{
	[Property] public int HotbarSlotCount { get; set; } = 6;
	[Property] public int BackpackSlotCount { get; set; } = 18;

	public List<InventoryItem> HotbarSlots { get; private set; } = new();
	public List<InventoryItem> BackpackSlots { get; private set; } = new();

	public InventoryItem HeldItem { get; private set; }

	public int SelectedHotbarIndex { get; private set; } = 0;

	protected override void OnStart()
	{
		for ( int i = 0; i < HotbarSlotCount; i++ )
		{
			HotbarSlots.Add( null );
		}

		for ( int i = 0; i < BackpackSlotCount; i++ )
		{
			BackpackSlots.Add( null );
		}

		// Test items
		AddItem( "Wood", 10 );
		AddItem( "Stone", 5 );
		AddItem( "Spear", 1 );
		AddItem( "Torch", 1 );
	}

	protected override void OnUpdate()
	{
	if ( Input.Pressed( "slot1" ) ) SelectHotbarSlot( 0 );
	if ( Input.Pressed( "slot2" ) ) SelectHotbarSlot( 1 );
	if ( Input.Pressed( "slot3" ) ) SelectHotbarSlot( 2 );
	if ( Input.Pressed( "slot4" ) ) SelectHotbarSlot( 3 );
	if ( Input.Pressed( "slot5" ) ) SelectHotbarSlot( 4 );
	if ( Input.Pressed( "slot6" ) ) SelectHotbarSlot( 5 );

	if ( Input.MouseWheel.y > 0 )
	{
		SelectPreviousHotbarSlot();
	}

	if ( Input.MouseWheel.y < 0 )
	{
		SelectNextHotbarSlot();
	}
	}

	public void AddItem( string itemName, int amount )
	{
		// First try to stack in hotbar
		foreach ( var item in HotbarSlots )
		{
			if ( item == null )
				continue;

			if ( item.Name == itemName )
			{
				item.Amount += amount;
				return;
			}
		}

		// Then try to stack in backpack
		foreach ( var item in BackpackSlots )
		{
			if ( item == null )
				continue;

			if ( item.Name == itemName )
			{
				item.Amount += amount;
				return;
			}
		}

		// Then try to place into empty hotbar slot first
		for ( int i = 0; i < HotbarSlots.Count; i++ )
		{
			if ( HotbarSlots[i] == null )
			{
				HotbarSlots[i] = new InventoryItem( itemName, amount );
				return;
			}
		}

		// If hotbar is full, place into backpack
		for ( int i = 0; i < BackpackSlots.Count; i++ )
		{
			if ( BackpackSlots[i] == null )
			{
				BackpackSlots[i] = new InventoryItem( itemName, amount );
				return;
			}
		}
	}

	public void RemoveHotbarItem( int slotIndex )
	{
		if ( slotIndex < 0 || slotIndex >= HotbarSlots.Count )
			return;

		HotbarSlots[slotIndex] = null;
	}

	public void RemoveBackpackItem( int slotIndex )
	{
		if ( slotIndex < 0 || slotIndex >= BackpackSlots.Count )
			return;

		BackpackSlots[slotIndex] = null;
	}

	public void ClickBackpackSlot( int slotIndex )
	{
	if ( slotIndex < 0 || slotIndex >= BackpackSlots.Count )
		return;

	var clickedItem = BackpackSlots[slotIndex];

	// Mouse empty + clicked item = pick item up
	if ( HeldItem == null && clickedItem != null )
	{
		HeldItem = clickedItem;
		BackpackSlots[slotIndex] = null;
		return;
	}

	// Mouse holding item + clicked empty slot = place item
	if ( HeldItem != null && clickedItem == null )
	{
		BackpackSlots[slotIndex] = HeldItem;
		HeldItem = null;
		return;
	}

	// Mouse holding item + clicked another item = swap
	if ( HeldItem != null && clickedItem != null )
	{
		BackpackSlots[slotIndex] = HeldItem;
		HeldItem = clickedItem;
		return;
	}
}

	public void ClickHotbarSlot( int slotIndex )
	{
	if ( slotIndex < 0 || slotIndex >= HotbarSlots.Count )
		return;

	var clickedItem = HotbarSlots[slotIndex];

	// Mouse empty + clicked item = pick item up
	if ( HeldItem == null && clickedItem != null )
	{
		HeldItem = clickedItem;
		HotbarSlots[slotIndex] = null;
		return;
	}

	// Mouse holding item + clicked empty slot = place item
	if ( HeldItem != null && clickedItem == null )
	{
		HotbarSlots[slotIndex] = HeldItem;
		HeldItem = null;
		return;
	}

	// Mouse holding item + clicked another item = swap
	if ( HeldItem != null && clickedItem != null )
	{
		HotbarSlots[slotIndex] = HeldItem;
		HeldItem = clickedItem;
		return;
	}
}

public void SelectHotbarSlot( int slotIndex )
{
	if ( slotIndex < 0 || slotIndex >= HotbarSlots.Count )
		return;

	SelectedHotbarIndex = slotIndex;
}

public void SelectNextHotbarSlot()
{
	if ( HotbarSlots.Count == 0 )
		return;

	SelectedHotbarIndex++;

	if ( SelectedHotbarIndex >= HotbarSlots.Count )
	{
		SelectedHotbarIndex = 0;
	}
}

public void SelectPreviousHotbarSlot()
{
	if ( HotbarSlots.Count == 0 )
		return;

	SelectedHotbarIndex--;

	if ( SelectedHotbarIndex < 0 )
	{
		SelectedHotbarIndex = HotbarSlots.Count - 1;
	}
}

public InventoryItem GetSelectedHotbarItem()
{
	if ( SelectedHotbarIndex < 0 || SelectedHotbarIndex >= HotbarSlots.Count )
		return null;

	return HotbarSlots[SelectedHotbarIndex];
}
}