﻿#if UNITY_EDITOR
@MenuItem("File/Load Scene [Additive]")
static function Apply ()
{
    var strScenePath : String = EditorUtility.GetAssetPath(Selection.activeObject);
    if (strScenePath == null)
    {
        EditorUtility.DisplayDialog("Select Scene", "You Must Select a Scene first!", "Ok");
        return;
    }
 
    Debug.Log("Opening " + strScenePath + " additively");
    EditorApplication.OpenSceneAdditive(strScenePath);
    return;
}