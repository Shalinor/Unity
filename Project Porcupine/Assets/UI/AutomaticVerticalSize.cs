using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]	//Naive way to run in editor... doesn't update (unless call is in void Update(){}), but is constantly running while editing. Button is a better method.
public class AutomaticVerticalSize : MonoBehaviour {

	public float childHeight = 35f;
	public float childWidth = 150f;

	// Use this for initialization
	void Start () {
		AdjustSize();
	}
	
	public void AdjustSize()
	{
		Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
		size.x = childWidth;
		size.y = this.transform.childCount * childHeight;
		this.GetComponent<RectTransform>().sizeDelta = size;
	}
}
