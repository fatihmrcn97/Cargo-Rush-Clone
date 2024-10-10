using UnityEditor;
using UnityEngine;
 
public class InspectorKeyboardInput
{ 
    [InitializeOnLoadMethod]
    static void EditorInit  ()
    {
        System.Reflection.FieldInfo info = typeof (EditorApplication).GetField ("globalEventHandler", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
 
        EditorApplication.CallbackFunction value = (EditorApplication.CallbackFunction)info.GetValue (null);
 
        value += EditorGlobalKeyPress;
 
        info.SetValue (null, value);
    }
 
 
    static void EditorGlobalKeyPress ()
    {
        switch (Event.current.keyCode)
        {
            case KeyCode.F7:
                Time.timeScale = 1;
                break;
            case KeyCode.F8:
                Time.timeScale = 3;
                break;
            case KeyCode.Alpha8:
                UIManager.instance.ScoreAdd(500);
                break;
            case KeyCode.Alpha7:
                UIManager.instance.ScoreAdd(-500);
                break;
            default:
                break;
        }
    }
}