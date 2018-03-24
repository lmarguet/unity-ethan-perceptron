using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Throw : MonoBehaviour {

	public GameObject spherePrefab;
	public GameObject cubePrefab;
	public Material green;
	public Material red;

	Perceptron perceptron;

	// Use this for initialization
	void Start () {
		perceptron = GetComponent<Perceptron>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("1")){
			ThrowPrefab(spherePrefab, red);
			perceptron.SendInput(0,0,0);
		}
		else if(Input.GetKeyDown("2"))
		{
			ThrowPrefab(spherePrefab, green);
			perceptron.SendInput(0,1,1);
		}
		else if(Input.GetKeyDown("3"))
		{
			ThrowPrefab(cubePrefab, red);
			perceptron.SendInput(1,0,1);
		}
		else if(Input.GetKeyDown("4"))
		{
			ThrowPrefab(cubePrefab, green);
			perceptron.SendInput(1,1,1);
		}else if (Input.GetKeyDown("space"))
		{
			perceptron.ResetTraining();
		}
		
	}

	private void ThrowPrefab(GameObject prefab, Material material)
	{
		GameObject g = Instantiate(prefab, Camera.main.transform.position, Camera.main.transform.rotation);
		g.GetComponent<Renderer>().material = material;
		g.GetComponent<Rigidbody>().AddForce(0, 0, 500);
		
		Destroy(g, 3);
	}
}

