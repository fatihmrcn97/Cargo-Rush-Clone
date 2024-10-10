using UnityEditor; 
public class CustomEditorSystem : Editor
{ 
    [MenuItem("Control Menu/NextLevel")]
    public static void NextLevel()
    {
        //  LevelManager.Instance.NextLevel();
    }
    
    [MenuItem("Control Menu/Level Completed")]
    public static void LevelCompleted()
    {
        //  LevelManager.Instance.LevelCompleted();
    } 
}

