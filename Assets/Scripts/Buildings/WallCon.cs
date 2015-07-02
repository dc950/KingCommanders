using UnityEngine;
using System.Collections;

public abstract class WallCon : Building {

	public WallCon(int maxHealth, Tile tile, Player owner) : base(maxHealth, tile, owner)
    {

    }


    public void Update()
    {
        DeleteObject();
        PlaceObject(true);
    }

    protected abstract void PlaceObject(bool updating);
}
