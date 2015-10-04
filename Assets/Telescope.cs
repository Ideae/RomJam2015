using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
            starObj.transform.position = new Vector3(star["x"].AsFloat, star["y"].AsFloat, star["z"].AsFloat)*0.05f;
          starObj.transform.position += starObj.transform.position.normalized;
            var str = star["label"].ToString().Split(' ');
            starObj.name = str[str.Length - 1];
            starObj.name = starObj.name.Substring(0, starObj.name.Length - 2);
            TextMesh t = starObj.GetComponentInChildren<TextMesh>();
            //t.text = starObj.name; //todo: uncomment. -zack
            starObj.transform.LookAt(this.transform);
            starObj.transform.localScale *= ( -(star["appmag"].AsFloat - 3.5f) *0.1f  + 1) ;
            //Camera.main.WorldToScreenPoint
            //print(starObj.name);
            //starObj.transform.localScale = starObj.transform.localScale / star["appmag"].AsFloat;
            //
            //starObj.transform.localScale /= 100f;
        }
        print("Done");

    }

  public UnityEngine.GameObject labelPrefab;
  public static Dictionary<Vector3, GameObject> activeStars = new Dictionary<Vector3, GameObject>();
  public int maxLifetime = 100;
	// Update is called once per frame
	void Update () {
    RaycastHit r = new RaycastHit();
	  if (Physics.Raycast(Vector3.zero, transform.forward*100f, out r, 100f)) {
	    var g = r.collider.gameObject;
	    var star = g.GetComponent<Star>();
	    if (star != null) {
	      print("hit star:" + star.name);
	      Vector3 dir = (g.transform.position - transform.position).normalized;
	      if (!activeStars.ContainsKey(dir)) {
          var labelGameobject = (GameObject)Instantiate(labelPrefab, dir, Quaternion.identity);
          activeStars[dir] = labelGameobject;
          labelGameobject.transform.LookAt(Vector3.zero);
          labelGameobject.GetComponentInChildren<TextMesh>().text = g.name;
	      }
        activeStars[dir].GetComponentInChildren<LabelScript>().OnRaycast(maxLifetime);
	    }
	  }




	}
}
