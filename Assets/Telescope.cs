using UnityEngine;
using System.Collections;
using SimpleJSON;
public class Telescope : MonoBehaviour {
    
    public float minBrightness;
    public float maxBrightness;
    public GameObject prefab;
    public TextAsset json; 
    void Start()
    {
        //string url = "http://star-api.herokuapp.com/api/v1/stars?min%5Bappmag%5D="+minBrightness+"&max%5Bappmag%5D=" + maxBrightness;
        print("Test;");
        JSONNode root;
        //WWW www = new WWW(url);
        //yield return www;
        print(json.text);
        root = JSONNode.Parse(json.text);
        print(root.AsArray.Count);
        foreach (var star in root.AsArray.Childs)
        {
            
            var starObj = GameObject.Instantiate(prefab);
            starObj.transform.position = new Vector3(star["x"].AsFloat, star["y"].AsFloat, star["z"].AsFloat).normalized ;
            var str = star["label"].ToString().Split(' ');
            starObj.name = str[str.Length - 1];
            starObj.name = starObj.name.Substring(0, starObj.name.Length - 2);
            starObj.transform.LookAt(this.transform);
            //Camera.main.WorldToScreenPoint
            //print(starObj.name);
            //starObj.transform.localScale = starObj.transform.localScale / star["appmag"].AsFloat;
            //
            //starObj.transform.localScale /= 100f;
        }
        print("Done");

    }

	// Update is called once per frame
	void Update () {
	
	}
}
