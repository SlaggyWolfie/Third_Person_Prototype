using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets.Resources.scripts
{
    [Serializable]
    public abstract class Component_ : MonoBehaviour
    {
        public int typeID = 0;
        public abstract void UpdateData(UnityEngine.Object unityObject);// where T : UnityEngine.Object;

        public void UpdateID()
        {
            typeID = GameObject_.GetID(GetType());
        }

        public virtual string Serialize()
        {
            return JsonUtility.ToJson(this, true);
        }
    }

    [Serializable]
    public class Transform_ : Component_
    {
        public static int count = 0;
        public static Dictionary<Transform, int> transformIDs = new Dictionary<Transform, int>();

        public Vector3 position = new Vector3();
        public Quaternion rotation = new Quaternion();
        public Vector3 scale = new Vector3();
        public int parentID = 0;
        public int selfID = 0;

        public override void UpdateData(UnityEngine.Object unityObject)
        {
            Transform transform = unityObject as Transform;
            if (!transform) return;

            position = transform.position;
            rotation = transform.rotation;
            scale = transform.lossyScale;

            selfID = count++;
            transformIDs[transform] = selfID;
        }

        public void UpdateParent()
        {
            if (transform.parent != null && transformIDs.ContainsKey(transform.parent))
                parentID = transformIDs[transform.parent];
        }
    }

    [Serializable]
    public class Light_ : Component_
    {
        public float intensity = 0;
        public int type = 0;
        public Vector4 color = new Vector4();

        public override void UpdateData(UnityEngine.Object unityObject)
        {
            Light light = unityObject as Light;
            if (!light) return;

            intensity = light.intensity;
            switch (light.type)
            {
                case LightType.Directional:
                    type = 0;
                    break;
                case LightType.Area:
                    type = 3;
                    break;
                case LightType.Point:
                    type = 1;
                    break;
                case LightType.Spot:
                    type = 2;
                    break;
                default:
                    type = 2;
                    break;
            }
            color = light.color;
        }
    }

    [Serializable]
    public class BoxCollider_ : Component_
    {
        public bool isTrigger = false;
        public Vector3 size = new Vector3();
        public Vector3 center = new Vector3();

        public override void UpdateData(UnityEngine.Object unityObject)
        {
            BoxCollider boxCollider = unityObject as BoxCollider;
            CapsuleCollider capsuleCollider = unityObject as CapsuleCollider;

            if (boxCollider)
            {
                isTrigger = boxCollider.isTrigger;
                size = boxCollider.size;
                center = boxCollider.center;
                return;
            }

            if (capsuleCollider)
            {
                isTrigger = capsuleCollider.isTrigger;
                size = new Vector3(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.radius);
                center = capsuleCollider.center;
                return;
            }
        }
    }

    [Serializable]
    public class MeshFilter_ : Component_
    {
        public string meshPath = null;

        public override void UpdateData(UnityEngine.Object unityObject)
        {
            MeshFilter meshFilter = unityObject as MeshFilter;
            if (!meshFilter) return;

            meshPath = AssetDatabase.GetAssetPath(meshFilter.sharedMesh);
        }
    }

    [Serializable]
    public class GameObject_ : Component_
    {
        //public delegate void LateSerializationDelegate();
        //public static event LateSerializationDelegate Late;

        //public static void CallLate()
        //{
        //    if (Late != null) Late();
        //}

        private static readonly Dictionary<Type, Type> toSerializableForm = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, int> serializableID = new Dictionary<Type, int>();
        private static bool _dictSetup = false;

        public static int GetID<T>() where T : Type
        {
            return GetID(typeof(T));
        }

        public static int GetID(Type type)
        {
            if (!_dictSetup) SetupDict();
            return serializableID[type];
        }

        public static bool Contains<T>() where T : Type
        {
            return Contains(typeof(T));
        }

        public static Type GetSerializableForm<T>()
            where T : Type
        {
            if (!_dictSetup) SetupDict();
            return GetSerializableForm(typeof(T));
        }

        public static bool Contains(Type type)
        {
            if (!_dictSetup) SetupDict();
            return toSerializableForm.ContainsKey(type);
        }

        public static Type GetSerializableForm(Type type)
        {
            if (!_dictSetup) SetupDict();
            return toSerializableForm[type];
        }

        private static void SetupDict()
        {
            toSerializableForm[typeof(GameObject)] = typeof(GameObject_);
            serializableID[typeof(GameObject_)] = 0;

            toSerializableForm[typeof(Transform)] = typeof(Transform_);
            serializableID[typeof(Transform_)] = 1;

            toSerializableForm[typeof(BoxCollider)] = typeof(BoxCollider_);
            toSerializableForm[typeof(CapsuleCollider)] = typeof(BoxCollider_);
            serializableID[typeof(BoxCollider_)] = 2;

            toSerializableForm[typeof(Light)] = typeof(Light_);
            serializableID[typeof(Light_)] = 3;

            toSerializableForm[typeof(MeshFilter)] = typeof(MeshFilter_);
            serializableID[typeof(MeshFilter_)] = 4;

            _dictSetup = true;
        }

        public new string name;
        public new string tag;
        public bool active = true;

        private bool _subscribedToLate = false;

        [SerializeField]
        public new Transform_ transform;
        [SerializeField]
        public List<Component_> components = new List<Component_>();

        public override void UpdateData(UnityEngine.Object unityObject)
        {
            //GameObject gameObject = unityObject as GameObject;
            //if (!gameObject) return;

            name = gameObject.name;
            tag = gameObject.tag;
            active = gameObject.activeInHierarchy;

            //if (!_subscribedToLate)
            //{
            //    Late += LateSerialization;
            //    _subscribedToLate = true;
            //}

            Component_[] components_ = gameObject.GetComponents<Component_>();

            foreach (Component_ comp in components_)
                if (!components.Contains(comp))
                    components.Add(comp);
            //components.AddRange(components_);
            components.Remove(this);

            transform = gameObject.GetComponent<Transform_>();
            transform.UpdateParent();
            components.Remove(transform);
            //throw new NotImplementedException();
        }

        public override string Serialize()
        {

            //"typeID": 0,
            //"name": "Pine_Tree_001 (4)",
            //"tag": "Untagged",
            //"active": true,
            //"transform": {
            //    "instanceID": -140062
            //},
            //"components": [
            //{
            //    "instanceID": -140064
            //}
            //]
            //string defaultString = JsonUtility.ToJson(this, true);
            //return defaultString;
            string data = "{\"GameObject\": {";
            data += "\"typeID\": " + typeID.ToString();
            data += ",\n";
            data += "\"name\": \"" + name.ToString();
            data += "\",\n";
            data += "\"tag\": \"" + tag.ToString();
            data += "\",\n";
            data += "\"active\": " + active.ToString().ToLower();
            data += ",\n";
            data += "\"transform\": " + transform.Serialize();
            data += ",\n";

            //data += JsonHelper.ToJson(components.ToArray(), true);
            data += "\"components\": ";
            data += "[";
            for (int i = 0; i < components.Count; i++)
            {
                data += components[i].Serialize();
                if (i < components.Count - 1) data += ",\n"; ;
            }
            //foreach (Component_ component in components)
            data += "]";

            data += "}}";
            return data;
        }
        //public void LateSerialization()
        //{
        //    Component_[] components_ = gameObject.GetComponents<Component_>();
        //    components.AddRange(components_);
        //    components.Remove(this);
        //}
    }

    public class SceneSerializer : MonoBehaviour
    {
        [MenuItem("Serialization/UpdateData")]
        private static void UpdateData()
        {
            //Debug.Log("Hello There");
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.name == "ThirdPersonController"/* && component is Collider*/) continue;

                //Debug.Log("Hello There1");
                var components = gameObject.GetComponents<Component>();
                foreach (Component component in components)
                {
                    if (component == null) Debug.Log("Null Component!?");
                    //Debug.Log("Hello There2");
                    if (gameObject.name == "ThirdPersonController"/* && component is Collider*/) continue;
                    if (gameObject.name.Contains("plate") && (component is Collider/* || component is MeshFilter*/)) continue;
                    if (component != null && GameObject_.Contains(component.GetType()))
                    {
                        //Debug.Log("Hello There3");
                        Type ourType = GameObject_.GetSerializableForm(component.GetType());
                        Component_ ourComponent = gameObject.GetComponent(ourType) as Component_;
                        if (ourComponent == null)
                            ourComponent = gameObject.AddComponent(ourType) as Component_;

                        if (ourComponent != null)
                        {
                            ourComponent.UpdateID();
                            ourComponent.UpdateData(component);
                        }
                        else Debug.Log("ERROR");
                    }
                }


                GameObject_ go_ = gameObject.GetComponent<GameObject_>();
                if (!go_) go_ = gameObject.AddComponent<GameObject_>();
                go_.UpdateID();
                go_.UpdateData(null);
            }

            Transform_.count = 0;
            //GameObject_.CallLate();
        }

        [MenuItem("Serialization/DestroyData")]
        private static void DestroyData()
        {
            //Debug.Log("Hello There");
            GameObject[] gameObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject gameObject in gameObjects)
            {
                //Debug.Log("Hello There1");
                var components = gameObject.GetComponents<Component_>();
                foreach (Component_ component in components)
                    DestroyImmediate(component);

                var componentsReal = gameObject.GetComponents<Component>();
                foreach (Component component in componentsReal)
                    if (component == null)
                        DestroyImmediate(component);
            }
        }

        [MenuItem("Serialization/Serialize")]
        private static void Serialize()
        {
            UpdateData();

            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            //string path = "Assets/Resources/" + Selection.activeObject.name + ".json";
            string path = "Assets/Resources/" + sceneName + ".json";
            File.WriteAllText(path, "");

            StreamWriter writer = new StreamWriter(path, true);

            GameObject_[] gameObjects = FindObjectsOfType<GameObject_>();
            Debug.Log(string.Format("Found {0} Game Objects for serialization.", gameObjects.Length));

            //string data = JsonHelper.ToJson(array, true);
            string data = "[";// = JsonUtility.ToJson(array[0], true);

            //foreach (GameObject_ go_ in gameObjects)
            //{
            //    data += go_.Serialize();
            //    data += ",\n";
            //}

            for (int i = 0; i < gameObjects.Length; i++)
            {
                data += gameObjects[i].Serialize();
                if (i < gameObjects.Length - 1) data += ",\n"; ;
            }

            data += "]";
            writer.Write(data);
            writer.Close();
        }
    }
}