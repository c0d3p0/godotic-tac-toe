using Godot;
using System;

public class FileUtil
{
    public static void Write(String filePath, String content)
    {
        File file = new File();
        file.Open(filePath, 3);
        file.StoreString(content);
        file.Close();
    }

    public static String ReadAsText(String filePath)
    {
        File file = new File();
        
        if(file.FileExists(filePath))
        {
            file.Open(filePath, 3);
            String fileData = file.GetAsText();
            file.Close();
            return fileData;
        }

        return null;
    }

    public static bool Exists(String filePath)
    {
        File file = new File();
        return file.FileExists(filePath);
    }

    public static void CreateFile(String filePath)
    {
        File file = new File();
        file.Open(filePath, 2);
        file.Close();
    }
}
