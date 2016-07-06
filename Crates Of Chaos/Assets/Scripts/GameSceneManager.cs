using UnityEngine;
using System.Collections;

public class GameSceneManager : MonoBehaviour {
    public GameObject SpawnerParent;
    public GameObject IslandParent;

    public static GameSceneManager Instance;

    void Awake() {
        Instance = this;
    }
}
