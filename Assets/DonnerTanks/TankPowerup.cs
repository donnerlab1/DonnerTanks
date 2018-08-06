using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankPowerup : MonoBehaviour {


    public GameObject Shield;
    public GameObject Speed;

    private bool shieldActive;
    private bool speedActive;

    public bool canShield;
    public bool canSpeed;

    public bool speedTrigger;
    public bool shieldTrigger;

    public Text speedText;
    public Text shieldText;

    public int m_PlayerNumber;
	// Use this for initialization
	void Start () {
        speedText = GameObject.Find("UI/P" + m_PlayerNumber + "/Speed").GetComponent<Text>();
        shieldText = GameObject.Find("UI/P" + m_PlayerNumber + "/Shield").GetComponent<Text>();
    }

    private void OnEnable()
    {
        canShield = true;
        canSpeed = true;
        
    }
    private void OnDisable()
    {
        canShield = false;
        canSpeed = false;
    }

    // Update is called once per frame
    void Update () {
		if(speedTrigger)
        {
            speedTrigger = false;
            if (canSpeed)
                ActivateSpeedBoost(5f);
        } else if (shieldTrigger)
        {
            shieldTrigger = false;
            if (canShield)
                ActivateShield(5f);
        }
    }
    
    public void ActivateSpeedBoost(float time)
    {
        speedActive = true;
        canSpeed = false;
        GetComponent<Complete.TankMovement>().speedMultiplier = 1.5f;
        Invoke("ResetSpeedCooldown", time+0.1f);
        Invoke("DeactivateSpeedBoost", time);
        Speed.SetActive(true);
        speedText.color = new Color(0, 0, 150);
    }

    private void DeactivateSpeedBoost()
    {
        GetComponent<Complete.TankMovement>().speedMultiplier = 1f;
        speedActive = false;
        Speed.SetActive(false);
        speedText.color = new Color(150, 0, 0);
    }

    private void ResetSpeedCooldown()
    {
        canSpeed = true;
        speedText.color = new Color(0, 150, 0);
    }
    public void ActivateShield(float time)
    {
        Debug.Log("activate Shield");
        shieldActive = true;
        canShield = false;
        GetComponent<Complete.TankHealth>().damageMultiplier = 0;
        Invoke("ResetShieldCooldown", time*2f);
        Invoke("DeactivateShield", time);
        Shield.SetActive(true);
        shieldText.color = new Color(0, 0, 150);
    }

    private void DeactivateShield()
    {
        GetComponent<Complete.TankHealth>().damageMultiplier = 1;
        shieldActive = false;
        Shield.SetActive(false);
        shieldText.color = new Color(150, 0, 0);
    }

    private void ResetShieldCooldown()
    {
        canShield = true;
        shieldText.color = new Color(0, 150, 0);
    }
}
