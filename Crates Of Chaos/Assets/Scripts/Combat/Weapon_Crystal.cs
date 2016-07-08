using UnityEngine;
using System.Collections;

public class Weapon_Crystal : Weapon
{
	public float base_projectile_speed = 1;
	
	private Targeter targeter;
	private LineRenderer linerenderer;
	private SpriteRenderer spriterenderer;
	private float scale = 0;
	
	void Start()
	{
		targeter = gameObject.GetComponentInChildren<Targeter>();
		linerenderer = gameObject.GetComponentInChildren<LineRenderer>();
		spriterenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
	}
	
	void Update()
	{
		if (targeter.current_target == null)
		{
			scale = Mathf.Max(0, scale - Time.deltaTime);
		}
		else
		{
			var direction = (targeter.current_target.transform.position - linerenderer.transform.position).normalized;
			var raystart = new Vector2(linerenderer.transform.position.x + direction.x * 2, linerenderer.transform.position.y + direction.y * 2);
			var raydir = new Vector2(direction.x, direction.y);
			var mask = (1 << LayerMask.NameToLayer("EnemyObject")) | (1 << LayerMask.NameToLayer("Default"));
			RaycastHit2D hit = Physics2D.Raycast(
				raystart,
				raydir,
				//renderer.transform.forward,
				50,
				mask);

			if (hit.collider == null)
			{
				Debug.DrawLine(linerenderer.transform.position - new Vector3(5, 0, 0), linerenderer.transform.position + new Vector3(5, 0, 0));
				//renderer.enabled = false;
				scale = Mathf.Max(0, scale - Time.deltaTime);
			}
			else
			{
				linerenderer.enabled = true;

				linerenderer.SetPosition(0, linerenderer.transform.position);
				linerenderer.SetPosition(1, hit.collider.transform.position);
				linerenderer.material.mainTextureOffset = new Vector2(linerenderer.material.mainTextureOffset.x - Time.deltaTime, 0);

				spriterenderer.transform.position = hit.collider.transform.position;
				scale = Mathf.Min(1, scale + Time.deltaTime);
			}
		}


		//Debug.DrawLine(renderer.transform.position, targeter.current_target.transform.position);
		//Debug.DrawLine(transform.position, targeter.current_target.transform.position);
		//Debug.DrawLine(renderer.transform.position, renderer.transform.position + renderer.transform.forward * 50);


		//		Debug.DrawLine(renderer.transform.position - new Vector3(0, 5, 0), renderer.transform.position + new Vector3(0, 5, 0));

		linerenderer.SetWidth(scale, scale);
		spriterenderer.transform.localScale = new Vector3(scale, scale, scale);
	}

	public override void Upgrade(int strength)
	{
		base_projectile_speed = base_projectile_speed * (1 + strength * 0.05f);
	}
}
