using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragKiller : MonoBehaviour, CentralizedTransformationObject {

	public float dragIgnoreTime = 1f; // time in seconds to ignore drag
	public float startActive = 0f;
	public float endActive = 0.5f;
	
	private BoxCollider trigger;

	// Use this for initialization
	void Start () 
	{
		trigger = GetComponent<BoxCollider>();
		trigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateCyclePosition(float cyclePos)
    {
		//Debug.Log("cycle pos: " + cyclePos);
        if (cyclePos >= startActive && cyclePos < endActive)
		{
			trigger.enabled = true;
		}
		else
			trigger.enabled = false;
    }
}
