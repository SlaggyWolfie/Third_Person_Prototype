using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class MyTransform
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
[System.Serializable]
public class GameObject_
{
    public string name;
    public string meshString;
    public int parentID;
    public int selfID;
    public Transform_ transform;
}

[System.Serializable]
public class Transform_
{
    public Vector3 position, rotation, scale;
}

[System.Serializable]
public class v3
{
    public float x = 0, y = 0, z = 0;
    public v3(Vector3 unityV)
    {
        x = unityV.x;
        y = unityV.y;
        z = unityV.z;
    }
}

public class sceneTojson : MonoBehaviour
{
    [MenuItem("Important Window/Delete Important Script")]
    private static void DeleteScript()
    {
        //GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("level1");

        for (int i = 0; i < gameObjects.Length; i++)
        {
           DestroyImmediate(gameObjects[i].GetComponent<ParentData>());
        }
    }

    [MenuItem("Important Window/Give Important Script")]
    private static void GiveScript()
    {
        //GameObject[] gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("level1");

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].GetComponent<ParentData>()==null)
            gameObjects[i].AddComponent<ParentData>();
        }
    }

    [MenuItem("Assets/Serialization/Write Scene")]
    private static void WriteToTextFile()
    {
        string path = "Assets/Resources/" + Selection.activeObject.name.ToString() + ".json";
        StreamWriter writer = new StreamWriter(path, true);

        //GameObject[] ObjArray = UnityEngine.Object.FindObjectsOfType<GameObject>();
        GameObject[] ObjArray = GameObject.FindGameObjectsWithTag("level1");
        int selfIndexIndex = 1;
        string data = "[";
        print(ObjArray.Length);
        for (int i = 0; i < ObjArray.Length; i++, selfIndexIndex++)
        {
            Transform_ objTransform = new Transform_(); 
            objTransform.position = ObjArray[i].transform.position;
            objTransform.rotation = ObjArray[i].transform.rotation.eulerAngles;
            objTransform.scale = ObjArray[i].transform.lossyScale;

            ObjArray[i].GetComponent<ParentData>().selfIndex = selfIndexIndex;
            ObjArray[i].GetComponent<ParentData>().parentIndex = 0;
            GameObject_ myObject = new GameObject_();
            myObject.transform = objTransform;
            myObject.name = ObjArray[i].gameObject.name;
            if (ObjArray[i].GetComponent<MeshFilter>()!= null)
            {
                myObject.meshString = ObjArray[i].GetComponent<MeshFilter>().sharedMesh.name;
            }

            if (ObjArray[i].transform.parent != null)
            {
                ObjArray[i].GetComponent<ParentData>().parentIndex = ObjArray[i].transform.parent.GetComponent<ParentData>().selfIndex;
            }

            myObject.parentID = ObjArray[i].GetComponent<ParentData>().parentIndex;
            myObject.selfID = ObjArray[i].GetComponent<ParentData>().selfIndex;
            if (i < ObjArray.Length-1)
                data += "{\"GameObject\":" + JsonUtility.ToJson(myObject) + "},\n";
            else
            {
                data += "{\"GameObject\":" + JsonUtility.ToJson(myObject) + "}\n";
            }
        }
        data += ']';
        writer.Write(data);
        writer.Close();

    }
    [MenuItem("Assets/Serialization/Read Scene")]
    private static void ReadToTextFile()
    {
        string path = "Assets/Resources/" + Selection.activeObject.name.ToString() + ".json";
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
    [MenuItem("Assets/Serialization/dont go here/Clear File")]
    private static void DeleteFile()
    {
        string path = "Assets/Resources/" + Selection.activeObject.name.ToString() + ".json";
        File.WriteAllText(path, "");
    }

}
