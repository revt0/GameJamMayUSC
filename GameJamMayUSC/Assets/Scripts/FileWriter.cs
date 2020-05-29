using System.IO;
using UnityEngine;

public class FileWriter : MonoBehaviour
{
    public static FileWriter Instance {get; set;}
    private string filePath;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        filePath = Application.persistentDataPath + "/connections.txt";
    }

    public void Write(string msg)
    {
        try
        {
            StreamWriter fileWriter = new StreamWriter(filePath, true);
            fileWriter.WriteLine(msg);
            fileWriter.Close();
        }
        catch
        {
            Debug.LogError($"cannot write to file: {filePath}");
        }
    }
}