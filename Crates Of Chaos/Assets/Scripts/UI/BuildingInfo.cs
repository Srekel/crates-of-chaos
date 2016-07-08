using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingInfo : MonoBehaviour {
	public Text textCost1;
	public Text textCost2;
	public Image imageCost1;
	public Image imageCost2;
	public Text towerName;
	public Text instructions;
	public Animator animator;
	public Text costLabel;
	public Text strengthLabel;
	private int basicCost;
	private int redCost;
	private int blueCost;


	private RadialButton.ButtonType currentType;

	public void Setup(RadialButton.ButtonType buttonType) {
		currentType = buttonType;
		costLabel.text = "Cost: ";
		strengthLabel.text = "";
		redCost = blueCost = 0;
		basicCost = 1;
		imageCost1.gameObject.SetActive (true);
		imageCost2.gameObject.SetActive (buttonType != RadialButton.ButtonType.BuildBasicShooter);
		if (buttonType == RadialButton.ButtonType.BuildBasicShooter) {
			towerName.text = "Attack Tower";
		} else if (buttonType == RadialButton.ButtonType.BuildRedShooter) {
			towerName.text = "Advanced Attack Tower";
			redCost = 1;
		} else if(buttonType == RadialButton.ButtonType.BuildBasicSpawner) {
			towerName.text = "Crystal Spawner";
			imageCost2.gameObject.SetActive (false);
			basicCost = 2;
		} else if(buttonType == RadialButton.ButtonType.DestroyTower) {
			towerName.text = "Destroy Tower";
			basicCost = 0;
			costLabel.text = "";
			imageCost1.gameObject.SetActive (false);
			imageCost2.gameObject.SetActive (false);
		}

		if (basicCost > 0)
			textCost1.text = basicCost.ToString ();
		else
			textCost1.text = "";

		if (redCost > 0)
			textCost2.text = redCost.ToString ();
		else if (blueCost > 0)
			textCost2.text = blueCost.ToString ();
		else
			textCost2.text = "";
	}

	void Start () {
	
	}

	void Update () {
	
	}

	public void Deactivate() {
		textCost1.text = "";
		textCost2.text = "";
		imageCost1.gameObject.SetActive (false);
		imageCost2.gameObject.SetActive(false);
		towerName.text = "";
		costLabel.text = "";
		strengthLabel.text = "";
	}
}
