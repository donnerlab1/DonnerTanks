using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour {

	public bool mineTrigger;
	public int targetPlayer;

	public float maxRange;

	public GameObject minePrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(mineTrigger){
			mineTrigger = false;
			spawnMine(targetPlayer);
		}
	}

	public void spawnMine(int player) {
		var tank = GetComponent<Complete.GameManager>().m_Tanks[player].m_Instance;
		var posX = Random.Range(-maxRange, maxRange);
		var posZ = Random.Range(-maxRange, maxRange);
		if(posX >= 0 && posX <=1 ) {
			posX+=2;
		}
		if(posZ >= 0 && posZ <=1 ) {
			posZ+=2;
		}
		if(posX <= 0 && posX >=-1 ) {
			posX-=2;
		}
		if(posZ <= 0  && posZ >=-1 ) {
			posZ-=2;
		}
		var position = tank.transform.position + new Vector3(posX,0,posZ);
		//Debug.Log(position);
		Instantiate(minePrefab, position, Quaternion.identity);
	}
}
