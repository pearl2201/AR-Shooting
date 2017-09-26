using UnityEngine;
using System.Collections;
using UnityEditor;
public class ResetPlayerPrefs
{

    [MenuItem("Help/ResetPrefs")]
    static void ActPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("GameObject/Create Other/SM/Create MusicManager")]
    static void CreateMusicManager()
    {
        GameObject gameObject = null;
        GameObject objectInit = Resources.Load<GameObject>("Music Manager");
        if (objectInit != null)
        {
            ClonePrefabResourceTool test = new ClonePrefabResourceTool();
            gameObject =  test.CreateGameObject(objectInit);
            test.SelfDestroy();
            
           

        }
        else
        {
            gameObject = new GameObject("Music Manager");
            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<MusicManager>();
        }
        gameObject.name = "Music Manager";
    }

    [MenuItem("GameObject/Create Other/SM/Create SoundFxManager")]
    static void CreateSoundFxManager()
    {
        GameObject gameObject = null;
        GameObject objectInit = Resources.Load<GameObject>("Sound Manager");
        if (objectInit != null)
        {
            ClonePrefabResourceTool test = new ClonePrefabResourceTool();
            gameObject = test.CreateGameObject(objectInit);
            test.SelfDestroy();



        }
        else
        {
            gameObject = new GameObject("Sound Manager");
            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<SoundManager>();
        }
        gameObject.name = "Sound Manager";
    }
}
