using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class MixAmoAssetImporter : AssetPostprocessor
{

	void OnPreprocessModel()
	{
		var modelImporter = assetImporter as ModelImporter;
		var asset = AssetDatabase.LoadAssetAtPath<Object>(assetImporter.assetPath);
		Debug.Log(asset == null);
		if (asset != null)
		{
			return;
		}
		MixAmoImportSetting importSetting = GetMixAmoImportSetting();
		if (importSetting != null)
		{
			
			if (IsAssetContainedInMixAmoDir(assetImporter.assetPath, importSetting))
			{
				modelImporter.animationType = ModelImporterAnimationType.Human;

				string path = System.IO.Path.GetDirectoryName(modelImporter.assetPath);
				modelImporter.ExtractTextures(path);
				modelImporter.materialLocation = ModelImporterMaterialLocation.External;
			}
		}

		AssetDatabase.ImportAsset(modelImporter.assetPath);
	}

	void OnPreprocessAnimation()
	{
		var modelImporter = assetImporter as ModelImporter;
		
		MixAmoImportSetting importSetting = GetMixAmoImportSetting();
		var asset = AssetDatabase.LoadAssetAtPath<Object>(assetImporter.assetPath);
		Debug.Log(asset == null);
		if (asset != null)
		{
			return;
		}
		if (importSetting != null && IsAssetContainedInMixAmoDir(assetImporter.assetPath, importSetting))
		{
			var animations = modelImporter.defaultClipAnimations;
			if (animations != null && animations.Length > 0)
			{
				int animationsCount = animations.Length;
				if (animationsCount > 2)
				{
					for (int i = 0; i < animationsCount; i++)
					{
						animations[i].name = System.IO.Path.GetFileNameWithoutExtension(modelImporter.assetPath) + "_" + i.ToString();
					}
				}
				else
				{
					animations[0].name = System.IO.Path.GetFileNameWithoutExtension(modelImporter.assetPath);
				}

				modelImporter.clipAnimations = animations;
			}

			modelImporter.animationType = ModelImporterAnimationType.Human;
			Avatar avatar = importSetting.avatar;

			if (avatar != null)
			{
				modelImporter.sourceAvatar = avatar;
			}

			/*var clips = AssetDatabase
							.LoadAllAssetRepresentationsAtPath(modelImporter.assetPath)
							.OfType<AnimationClip>()
							.ToArray<AnimationClip>();

			foreach (var oldClip in clips)
			{
				var newClip = new AnimationClip();
				AnimationUtility.SetAnimationEvents(newClip, AnimationUtility.GetAnimationEvents(oldClip));
				AnimationUtility.SetAnimationClipSettings(newClip, AnimationUtility.GetAnimationClipSettings(oldClip));

				
				foreach (var curveBinding in AnimationUtility.GetCurveBindings(oldClip))
				{
					AnimationCurve curve = AnimationUtility.GetEditorCurve(oldClip, curveBinding);
					newClip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, curve);
				}

				foreach (var curveBinding in AnimationUtility.GetObjectReferenceCurveBindings(oldClip))
				{
					AnimationCurve curve = AnimationUtility.GetEditorCurve(oldClip, curveBinding);
					newClip.SetCurve(curveBinding.path, curveBinding.type, curveBinding.propertyName, curve);
				}

				var path = System.IO.Path.GetDirectoryName(modelImporter.assetPath) + "/" + oldClip.name + ".anim";
				path = path.Replace("\\", "/");
				AssetDatabase.CreateAsset(newClip, path);
			}*/
			//AssetDatabase.Refresh();
			
		}
	}

	MixAmoImportSetting GetMixAmoImportSetting()
	{
		string[] importSettingPaths = AssetDatabase.FindAssets("t:MixAmoImportSetting");
		if (importSettingPaths.Length > 0)
		{
			var importSetting = AssetDatabase.LoadAssetAtPath<MixAmoImportSetting>(AssetDatabase.GUIDToAssetPath(importSettingPaths[0]));
			return importSetting;
		}

		return null;
	}

	bool IsAssetContainedInMixAmoDir(string assetPath, MixAmoImportSetting importSetting)
	{
		var allDirectories = new List<string>();
		allDirectories.AddRange(System.IO.Directory.GetDirectories(importSetting.MixAmoDirectory, "*", System.IO.SearchOption.AllDirectories));
		allDirectories.Add(importSetting.MixAmoDirectory);
		for (int i = 0; i < allDirectories.Count; i++)
		{
			allDirectories[i] = FileUtil.GetProjectRelativePath(allDirectories[i].Replace("\\", "/"));
		}

		string processedAssetPath = System.IO.Path.GetDirectoryName(assetPath).Replace("\\", "/");

		return allDirectories.Contains(processedAssetPath);
	}
}
