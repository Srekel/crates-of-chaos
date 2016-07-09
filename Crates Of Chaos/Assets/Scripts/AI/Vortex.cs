using UnityEngine;
using System.Collections;

public class Vortex : MonoBehaviour {
	public int levels = 10;
	public float time_until_first_spawn = 3;
	public float time_between_levels = 3;
	public float time_between_spawns = 10f;

	public GameObject enemy_1_prefab;
	public GameObject enemy_2_prefab;

	public int level = 1;
	private float time_to_next;
	private string state = "waiting";
	private string state_after_wait = "opening";
	private int to_spawn;
	private bool do_levelup = false;

	private float time_to_text = 0;
	private string text;
	private float time_spawning = 0;
	private float textscale = 1;

	private GUIStyle style;
	private Texture2D bg;

	// Use this for initialization
	void Start () {
		time_to_next = time_until_first_spawn;
		transform.localScale = new Vector3(1, 1, 1) * 0.5f;

		bg = new Texture2D(1, 1);
		bg.SetPixel(0, 0, new Color(1, 1, 0, 0.5f));
		bg.wrapMode = TextureWrapMode.Repeat;
		bg.Apply();

		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.black;
		style.normal.background = bg;
	}

	internal void levelup()
	{
		do_levelup = true;
	}

	// Update is called once per frame
	void Update () {
		if (state == "waiting")
		{
			if (Time.time > time_to_next)
			{ 
				state = state_after_wait;
			}
		}

		if (state == "opening")
		{
			var dt = Time.deltaTime;
			transform.localScale += new Vector3(dt, dt, dt);
			if (transform.localScale.x >= 4)
			{
				transform.localScale = new Vector3(4, 4, 4);
				state = "waiting";
				state_after_wait = "spawning";
				time_to_next = Time.time + 1;

				to_spawn = level;
				time_between_spawns *= 0.9f;
			}
		}

		if (state == "spawning")
		{
			if (do_levelup)
			{
				do_levelup = false;
				level++;
				if (level == levels)
				{
					state = "waiting";
					state_after_wait = "dying";
					time_to_next = Time.time + 1;

				}
				else
				{
					state = "waiting";
					state_after_wait = "closing";
					time_to_next = Time.time + 1;
				}
			}
			else if (Time.time > time_to_next)
			{
				var offset = new Vector3(
					Random.value * 0.2f - 0.1f,
					Random.value - 0.5f,
					0) * 5;
				var pos = transform.position + offset;
				pos.z = 0;
				if (Random.value > 0.5)
				{
					Instantiate(enemy_1_prefab, pos, Quaternion.identity);
				}
				else
				{
					Instantiate(enemy_2_prefab, pos, Quaternion.identity);
				}
				
				time_to_next = Time.time + time_between_spawns;
			}
		}

		if (state == "closing")
		{
			var dt = Time.deltaTime;
			transform.localScale -= new Vector3(dt, dt, dt) * 5;
			if (transform.localScale.x <= 0.5f)
			{
				transform.localScale = new Vector3(1, 1, 1) * 0.5f;
				state = "waiting";
				state_after_wait = "opening";
				time_to_next = Time.time + time_between_levels;
			}
		}

		if (state == "dying")
		{
			var dt = Time.deltaTime;
			transform.localScale -= new Vector3(dt, dt, dt) * 1;
			transform.Rotate(new Vector3(0, 0, Time.deltaTime * 360));
			if (transform.localScale.x <= 0.1f)
			{
				transform.localScale = new Vector3(1, 1, 1) * 0.1f;
				state = "dead";
			}
		}
	}

	void drawtext(string text, float fontsize, int height)
	{
		style.fontSize = (int)fontsize;

		GUI.Label(
			new Rect(0, 100, Screen.width, height),
			text, style);

	}

	void OnGUI()
	{
		float scale = 1f + 0.05f * Mathf.Sin(Time.time * 3) * textscale;
		//textscale = Mathf.Max(0, textscale - Time.deltaTime);

		if (state == "opening")
		{
			drawtext("Rift opens!", 150 * scale, 200);
			time_to_text = 3;
		}

		if (text != null)
		{
			drawtext(text, 70 * scale, 150);

			time_to_text -= Time.deltaTime;
			if (time_to_text <= 0)
			{
				text = null;
			}
		}
		else if (state == "spawning" && time_to_text > 0)
		{
			drawtext("Wave " + level.ToString(), 70 * scale, 150);
			time_to_text -= Time.deltaTime;
			
			if (time_to_text <= 0)
			{
				time_to_text = 10;
				text = "Wave Goal: Sacrifice a level 3 tower with Strength " + GameState.instance.needed_sacrifice;
			}
		}
		else if (state == "spawning")
		{
			time_spawning += Time.deltaTime;
			if (time_spawning > 60)
			{
				drawtext("Remember: Sacrifice a tower with Strength " + GameState.instance.needed_sacrifice, 50 * scale, 100);
			}
			if (time_spawning > 65)
			{
				time_spawning = 0;
			}
		}

		if (state == "closing")
		{
			time_to_text -= Time.deltaTime;

			drawtext("Wave " + level.ToString() + " complete!" + level, 50 * scale, 100);
		}
	}
}
