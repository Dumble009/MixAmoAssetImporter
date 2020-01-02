using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "MixAmoTargetAvatar")]
public class MixAmoImportSetting : ScriptableObject
{
	public Avatar avatar;
	[SerializeField]
	string mixAmoDirectory;
	public string MixAmoDirectory {
		get {
			return Application.dataPath + "/" + mixAmoDirectory;
		}
	}
	public bool isAnimationMakeLoop;
}


