using UnityEngine;
using System.Collections;

public abstract class Spawner : Building {

    public Tile spawnTile;

    public Spawner(int maxHealth, Tile tile) : base(maxHealth, tile)
    {

    }
	
}
