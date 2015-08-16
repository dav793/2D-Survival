using UnityEngine;
using System.Collections;

public class GActor : GObject {
	
	public WorldSector sector;
	public BOOL_YN environmental;
	public BOOL_YN npc;

	public GActor_RefList_Index reflist_index;

	public GActor(BOOL_YN environmental, BOOL_YN npc) {
		base.type = GObjectType.Actor;	
		this.environmental = environmental;
		this.npc = npc;
		reflist_index = new GActor_RefList_Index ();
	}

	public void addToSector(WorldSector sector) {
		if (this.sector == null) {
			// object does not belong to a sector
			this.sector = sector;
			sector.Contained_Objects.addObject(this);
		}
	}

	public void removeFromSector() {
		if (sector != null) {
			// object belongs to a sector
			sector.Contained_Objects.removeObject(this);
			sector = null;
		}
		
	}
	
	public void transferToSector(WorldSector sector) {
		if (this.sector != null) {
			// object belongs to a sector
			removeFromSector();
		}
		addToSector (sector);
	}

}
