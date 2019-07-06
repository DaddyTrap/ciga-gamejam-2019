
using UnityEngine;
using UnityEditor;

public class WaveScriptableImporter : EditorWindow {

    private string jsonStr;

    WaveScriptableImporter() {
        this.titleContent = new GUIContent("Wave Scriptable Importer");
    }

    [MenuItem("Import/Import Wave")]
    static void showWindow() {
        EditorWindow.GetWindow(typeof(WaveScriptableImporter));
    }

    void OnGUI() {
        jsonStr = GUILayout.TextArea(jsonStr, GUILayout.MaxHeight(75));

        if (GUILayout.Button("Import")) {
            Import();
        }
    }

    void Import() {

        // 读取 json
        var waves = JsonUtility.FromJson<WaveScriptable.WaveScriptableWrapper>(jsonStr);
        WaveScriptable asset = ScriptableObject.CreateInstance<WaveScriptable>();
        asset.waves = waves.waves;
        AssetDatabase.CreateAsset(asset, "Assets/New.asset");
        AssetDatabase.SaveAssets();

        Selection.activeObject = asset;
    }
}
