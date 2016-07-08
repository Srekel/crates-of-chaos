using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour {
    public Sprite RedCrystal;
    public Sprite BlueCrystal;
    public Sprite BasicCrystal;

    public Sprite BasicTower;
    public Sprite BlueTower;
	public Sprite RedTower;

	public Sprite CrystalTower;
	public Sprite Cancel;

    public static SpriteManager Instance;
    

	void Start () {
        if (Instance != null)
            Destroy(this);
        DontDestroyOnLoad(this);
        Instance = this;
	}
	
}
