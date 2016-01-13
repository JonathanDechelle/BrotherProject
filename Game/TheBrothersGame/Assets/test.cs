using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    public GameObject m_Pivot;

	// Update is called once per frame
	void Update ()
    {
        this.transform.RotateAround(m_Pivot.transform.position, Vector3.up, -20);
	}
}
