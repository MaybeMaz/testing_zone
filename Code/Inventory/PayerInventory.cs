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

	public InventoryItem SelectedHotbarItem
	{
		get
		{
			if ( SelectedHotbarIndex < 0 || SelectedHotbarIndex >= HotbarSlots.Count )
				return null;

			return HotbarSlots[SelectedHotbarIndex];
		}
	}

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
		HandleHotbarSelectionInput();
	}

	private void HandleHotbarSelectionInput()
	{
		if ( HotbarSlots.Count <= 0 )
			return;

		// Number keys 1, 2, 3, 4, 5, 6
		for ( int i = 0; i < HotbarSlots.Count; i++ )
		{
			string keyName = (i + 1).ToString();

			if ( Input.Pressed( keyName ) )
			{
				SelectHotbarSlot( i );
				return;
			}
		}

		// Mouse wheel
		if ( Input.MouseWheel.y > 0 )
		{
			SelectPreviousHotbarSlot();
		}
		else if ( Input.MouseWheel.y < 0 )
		{
			SelectNextHotbarSlot();
		}
	}

	public void SelectHotbarSlot( int slotIndex )
	{
		if ( slotIndex < 0 || slotIndex >= HotbarSlots.Count )
			return;

		SelectedHotbarIndex = slotIndex;

		var item = SelectedHotbarItem;

		if ( item == null )
		{
			Log.Info( $"Selected hotbar slot {SelectedHotbarIndex + 1}: Empty" );
		}
		else
		{
			Log.Info( $"Selected hotbar slot {SelectedHotbarIndex + 1}: {item.Name} x{item.Amount}" );
		}
	}

	public void SelectNextHotbarSlot()
	{
		if ( HotbarSlots.Count <= 0 )
			return;

		int nextIndex = SelectedHotbarIndex + 1;

		if ( nextIndex >= HotbarSlots.Count )
			nextIndex = 0;

		SelectHotbarSlot( nextIndex );
	}

	public void SelectPreviousHotbarSlot()
	{
		if ( HotbarSlots.Count <= 0 )
			return;

		int previousIndex = SelectedHotbarIndex - 1;

		if ( previousIndex < 0 )
			previousIndex = HotbarSlots.Count - 1;

		SelectHotbarSlot( previousIndex );
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

		Log.Info( "Inventory is full!" );
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
		Log.Info( $"Clicked backpack slot {slotIndex}" );

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
		Log.Info( $"Clicked hotbar slot {slotIndex}" );

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
}