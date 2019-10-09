using System;

using Godot;


public class JSONUtil : Node
{
    public static T ParseAs<T>(string data)
    {
        return (T) JSON.Parse(data).Result;
    }

    public static T ParseFileAs<T>(string filePath)
    {
        return (T) JSON.Parse(FileUtil.ReadAsText(filePath)).Result;
    }

    public static void WriteToJSONFile<T>(T data, String filePath)
    {
        FileUtil.Write(filePath, JSON.Print(data));
    }
}
