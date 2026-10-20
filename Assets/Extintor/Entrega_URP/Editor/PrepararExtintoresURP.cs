using System.IO;
using UnityEditor;
using UnityEngine;

// Coloque a pasta Entrega_URP dentro de Assets. Execute pelo menu Tools/IC.
public static class PrepararExtintoresURP
{
    [MenuItem("Tools/IC/Criar extintores URP")]
    public static void Criar()
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) { Debug.LogError("O shader URP Lit não está disponível neste projeto."); return; }
        string root = null;
        foreach (var guid in AssetDatabase.FindAssets("PrepararExtintoresURP t:MonoScript"))
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (Path.GetFileName(path) == "PrepararExtintoresURP.cs")
                root = Path.GetDirectoryName(Path.GetDirectoryName(path)).Replace('\\', '/');
        }
        if (root == null) { Debug.LogError("Pasta de entrega não encontrada."); return; }
        var model = AssetDatabase.LoadAssetAtPath<GameObject>(root + "/Extintor.fbx");
        if (model == null) { Debug.LogError("Extintor.fbx não encontrado junto da pasta Editor."); return; }
        var dest = root + "/Gerados_URP";
        // Uma nova pasta preserva materiais/prefabs que o usuário já ajustou.
        dest = AssetDatabase.GenerateUniqueAssetPath(dest);
        AssetDatabase.CreateFolder(root, Path.GetFileName(dest));
        var red = Make(shader, dest, "Pintura_Vermelha", new Color(0.65f, 0.07f, 0.09f), 0, 0.52f);
        var rubber = Make(shader, dest, "Borracha", new Color(0.13f, 0.14f, 0.15f), 0, 0.22f);
        var metal = Make(shader, dest, "Metal", new Color(0.74f, 0.76f, 0.78f), 0.85f, 0.7f);
        var gauge = Make(shader, dest, "Mostrador", new Color(0.89f, 0.90f, 0.86f), 0, 0.4f);
        foreach (var key in new[] { "A", "CO2", "Po_BC" })
        {
            string texturePath = root + "/Textures/Extintor_" + key + "_BaseColor.png";
            var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null) { Debug.LogError("Textura ausente: " + texturePath); continue; }
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = true;
            importer.alphaSource = TextureImporterAlphaSource.None;
            importer.mipmapEnabled = true;
            importer.maxTextureSize = 4096;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.anisoLevel = 4;
            importer.textureCompression = TextureImporterCompression.CompressedHQ;
            importer.SaveAndReimport();
            var bodyMat = Make(shader, dest, "Corpo_" + key, Color.white, 0, 0.52f);
            bodyMat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath));
            EditorUtility.SetDirty(bodyMat);
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
            try
            {
                instance.name = "Extintor_" + key;
                foreach (var renderer in instance.GetComponentsInChildren<Renderer>(true))
                {
                    var material = metal;
                    switch (renderer.gameObject.name)
                    {
                        case "Corpo": material = bodyMat; break;
                        case "Mangueira": case "Base": material = rubber; break;
                        case "Alavanca": case "Gatilho": material = red; break;
                        case "Mostrador": material = gauge; break;
                    }
                    var slots = renderer.sharedMaterials;
                    for (int i = 0; i < slots.Length; i++) slots[i] = material;
                    renderer.sharedMaterials = slots;
                }
                PrefabUtility.SaveAsPrefabAsset(instance, dest + "/Extintor_" + key + ".prefab");
            }
            finally { Object.DestroyImmediate(instance); }
        }
        AssetDatabase.SaveAssets();
        Debug.Log("Extintores criados em " + dest + ". Ajuste a escala do prefab conforme seu cenário.");
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(dest);
    }
    static Material Make(Shader shader, string folder, string name, Color color, float metallic, float smoothness)
    {
        var mat = new Material(shader) { name = name };
        mat.SetColor("_BaseColor", color);
        mat.SetFloat("_Metallic", metallic);
        mat.SetFloat("_Smoothness", smoothness);
        AssetDatabase.CreateAsset(mat, folder + "/" + name + ".mat");
        return mat;
    }
}
