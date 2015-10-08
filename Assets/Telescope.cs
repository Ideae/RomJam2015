using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Linq;

public class Telescope : MonoBehaviour {
    
    public float minBrightness;
    public float maxBrightness;
    public GameObject prefab;
    public TextAsset starJson, constellationsJson, starunionsJson;
    public static IEnumerable<IGrouping<string, JSONNode>> galaxyDict;
    public static Dictionary<string, GameObject> starDict = new Dictionary<string, GameObject>();
    public static Dictionary<string, List<GameObject>> starLists = new Dictionary<string, List<GameObject>>();
    public static List<LineRenderer> lines = new List<LineRenderer>();
    public static int linecount = 1;
    public GameObject linePrefab;
    void Start()
    {
        //string url = "http://star-api.herokuapp.com/api/v1/stars?min%5Bappmag%5D="+minBrightness+"&max%5Bappmag%5D=" + maxBrightness;
        print("Test;");
        JSONNode starsRoot;
        //WWW www = new WWW(url);
        //yield return www;
        //print(starJson.text);
        starsRoot = JSONNode.Parse(starJson.text);
        //print(starsRoot.AsArray.Count);
        foreach (var star in starsRoot.AsArray.Childs)
        {
            
            var starObj = GameObject.Instantiate(prefab);
            starObj.transform.position = new Vector3(star["x"].AsFloat, star["y"].AsFloat, star["z"].AsFloat)*0.05f;
            starDict.Add(star["id"], starObj);
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
            //GameObject g = new GameObject("line" + linecount++);
            //var lr = g.AddComponent<LineRenderer>();
            GameObject g = Instantiate(linePrefab);
            g.name = "line" + linecount++;
            lines.Add(g.GetComponent<LineRenderer>());
        }
        print("Done");

        JSONNode constellationsRoot;
        //WWW www = new WWW(url);
        //yield return www;
        //print(starJson.text);
        constellationsRoot = JSONNode.Parse(constellationsJson.text);
        //print(starsRoot.AsArray.Count);

        galaxyDict = constellationsRoot.AsArray.Childs.GroupBy(a => a["galaxy"].ToString());
        foreach(var gal in galaxyDict)
        {
            var galkey = gal.Key;
            
            print("--gal: " + galkey);
            foreach (var con in gal)
            {
                print("con: " + con["name"]);

            }
        }

        JSONNode starunionsRoot;
        //WWW www = new WWW(url);
        //yield return www;
        //print(starJson.text);
        starunionsRoot = JSONNode.Parse(starunionsJson.text);
        //print(starsRoot.AsArray.Count);
        
        foreach (var star in starunionsRoot.AsArray.Childs)
        {
            var starid = star["star_id"];
            var conid = star["constellation_id"];
            if (starDict.ContainsKey(starid))
            {
                if (!starLists.ContainsKey(conid))
                {
                    starLists[conid] = new List<GameObject>();
                }
                starLists[conid].Add(starDict[starid]);
            }
        }



        SetLines();

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
	      //print("hit star:" + star.name);
	      Vector3 dir = (g.transform.position - transform.position).normalized;
	      if (!activeStars.ContainsKey(dir)) {
              var labelGameobject = (GameObject)Instantiate(labelPrefab, dir, Quaternion.identity);
              activeStars[dir] = labelGameobject;
              labelGameobject.transform.LookAt(Vector3.zero);
              labelGameobject.GetComponentInChildren<TextMesh>().text = g.name;
	        }
            if (activeStars[dir] == null)
            {
                activeStars.Remove(dir);
            }
            else
            {
                activeStars[dir].GetComponentInChildren<LabelScript>().OnRaycast(maxLifetime);
            }
	    }
	  }
	}

    void SetLines()
    {
        int counter = 0;

        foreach (var galkey in starLists.Keys)
        {
            var gal = starLists[galkey];
            //var galkey = gal.Key;
            //print("--gal: " + galkey);
            //foreach (var con in gal)
            //{
            //    //print("con: " + con["name"]);
            //}
            int c = gal.Count;
            
            for(int i = 0; i < c-1; i++)
            {
                //var a = gal[i]["id"];
                //var b = gal.ElementAt(i+1)["id"];
                //if (!starDict.ContainsKey(a) || !starDict.ContainsKey(b))
                //{
                //    print("couldnt find star in dictionary");
                //    continue;
                //}
                var star1 = gal[i];

                var star2 = gal[i+1];
                Vector3 va = star1.transform.position;
                Vector3 vb = star2.transform.position;
                var lr = lines[counter++];
                lr.SetPosition(0, va);
                lr.SetPosition(1, vb);

                //Vector3 sum = Vector3.zero;
            }
        }
    }
}
