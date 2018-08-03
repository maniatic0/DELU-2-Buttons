using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Revive : MonoBehaviour
{
	[SerializeField]
	private Text reviveText;
    private string buttonLeft = "Left";

    private string buttonRight = "Right";
    // Use this for initialization
    void Start()
    {
        this.enabled = false;
		reviveText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(buttonLeft) || Input.GetButtonDown(buttonRight))
        {
            PlayerLife.player.Revive();
            Start();
        }
    }

    public void OnDeath()
    {
        this.enabled = true;
		reviveText.enabled = true;
    }
}
