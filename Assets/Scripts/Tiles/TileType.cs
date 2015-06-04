using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileType {

	public string name;
	public GameObject visualPrefab;
	
	public bool canWalk = true;
	public bool canBuild = true;
	public int speedFactor = 1;
	
}
