using UnityEditor;
using UnityEngine;

public class ForcePrefabPreview
{
    [MenuItem("Assets/Force Generate Preview")]
    public static void Generate()
    {
        foreach (GameObject prefab in Selection.gameObjects)
        {
            Texture2D preview = AssetPreview.GetAssetPreview(prefab);

            if (preview != null)
            {
                Texture2D customIcon = new Texture2D(preview.width, preview.height, preview.format, false);
                Graphics.CopyTexture(preview, customIcon);

                EditorGUIUtility.SetIconForObject(prefab, customIcon);
                EditorUtility.SetDirty(prefab);
            }
        }
        AssetDatabase.SaveAssets();
    }
}