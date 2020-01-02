# MixAmoAssetImporter
MixAmo is a useful web service where you can download 3D models and animations for nothing.
But to use these assets properly in Unity needs some operations.
This AssetPostProcessor helps you with these operations.


# How to use
You must configure MixAmoImportSetting.asset in MixAmoAssetImporter/Editor.
1. Set a humanoid avatar in Avatar property.
2. Set a path where you want to import assets from MixAmo in MixAmoDirectory property.<br>
   (if you want to import to "(projectpath)/Assets/ExternalAssets/MixAmo", set a path "ExternalAssets/MixAmo")
![img](https://i.imgur.com/HC5cak1.png)

<br>
If you happened to delete MixAmoImportSetting.asset, you can create alternative one,<br>
Right click on Project view → Create → MixAmoImportSetting
<br><br>
You can store MixAmoImportSetting.asset where you want, but you mustn't have more than two.

Please make sure that fbx files of characters don't have any animations.
When you download characters from Mixamo, don't forget to set "Pose" as "Original Pose(.fbx)".
If you set "Pose" as "T-Pose", this asset postprocessor recognize that file as animation file, and change the avatar.
