using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour
{

	public Vortex enemy_rift;
	public GameObject headquarters;

	public int needed_sacrifice = 1;

	private string state = "running";

	public static GameState instance;
	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (headquarters == null)
		{
			state = "lost";
		}
	}

	public bool CanSacrifice(GameObject go)
	{
		var upgradable = go.GetComponentInChildren<Upgradable>();
		return upgradable.strength >= needed_sacrifice && upgradable.level == 3;
	}

	public void Sacrifice(GameObject go)
	{
		needed_sacrifice *= 2;
		GameObject.Destroy(go);
		enemy_rift.levelup();

		if (enemy_rift.level == enemy_rift.levels)
		{
			state = "won";
		}
	}


	void OnGUI()
	{
		if (state == "lost")
		{
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 150;

			GUI.Box(
				new Rect(0, Screen.height / 2 - 200, Screen.width, 400),
				"You lost!", style);
		}
		else if (state == "won")
		{
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 150;

			GUI.Box(
				new Rect(0, Screen.height / 2 - 200, Screen.width, 400),
				"You won!", style);
		}
	}
}
