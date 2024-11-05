//place this script in the Editor folder within Assets.
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
//to be used on the command line:
//$ Unity -quit -batchmode -executeMethod WebGLBuilder.build

public class WebGLBuilder
{
    [MenuItem("MyTools/Build")]
    public static void BuildGame()
    {
        string path = Application.dataPath + "..\\Builds\\";
        string[] scenes = new string[SceneManager.sceneCountInBuildSettings];
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            scenes[i] = System.IO.Path.GetFullPath(SceneUtility.GetScenePathByBuildIndex(i));
        }
        print(path);
        //BuildPipeline.BuildPlayer(scenes, path + "Web\\", BuildTarget.WebGL, BuildOptions.Development);
        // BuildPipeline.BuildPlayer(scenes, path + "Linux\\CTS.x86_64", BuildTarget.StandaloneLinux64, BuildOptions.None);
        BuildPipeline.BuildPlayer(scenes, path + "Windows\\CTS.exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}