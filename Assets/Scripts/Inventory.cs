using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
   public class Slot
   {
	   public CollectableType type;
	   public int count;
	   public int maxAllowed;

	   public Slots()
	   {
		   type = CollectableType.NONE;
		   count = 0;
		   maxAllowed = 99;
	   }

	   public bool CanAddItem()
	   {
		   if()
	   }
   }

   public List<Slot> slots = new List<Slot>();

   public Inventory(int numSlots)
   {
	   for(int i =0; i < numSlots; i++)
	   {
		   Slot slot = new Slot();
		   slots.Add(slot);
	   }
   }
   public void Add(CollectableType typeToAdd)
   {
	   foreach(Slot slot in slots)
	   {
		   if(slot.type == typeToAdd &&)
	   }
   }
}
