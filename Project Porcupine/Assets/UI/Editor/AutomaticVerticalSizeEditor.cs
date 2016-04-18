using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(AutomaticVerticalSize))]
public class AutomaticVerticalSizeEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		if( GUILayout.Button("Recalc Size") )
		{
			//Equivalent - we are casting target to the script, which we can then call public functions, etc
/*			AutomaticVerticalSize myScript = ((AutomaticVerticalSize)target);
			myScript.AdjustSize();*/

			((AutomaticVerticalSize)target).AdjustSize();
		}
	}
}
