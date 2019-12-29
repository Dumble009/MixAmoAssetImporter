﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MixAmoAssetImporter : AssetPostprocessor
{
	void OnPreprocessModel()
	{
		var modelImporter = assetImporter as ModelImporter;
		
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
	}

	void OnPreprocessAnimation()
	{
		var modelImporter = assetImporter as ModelImporter;
		MixAmoImportSetting importSetting = GetMixAmoImportSetting();

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
