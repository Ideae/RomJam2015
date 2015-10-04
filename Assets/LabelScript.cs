using UnityEngine;
using System.Collections;

public class LabelScript : MonoBehaviour {
  private int lifetime = -1;
  private int maxlife;
  // Use this for initialization
  void Start() {
    //origColor = GetComponentInChildren<SpriteRenderer>().color;
    //oCT = GetComponentInChildren<TextMesh>().color;
    origColor = Color.white;
    oCT = Color.white;
  }

  private Color origColor, oCT;
  void Update()
  {
    if (lifetime >= 0)
    {
      if (--lifetime == 0)
      {
        lifetime = -1;
        //remove label
        Telescope.activeStars.Remove(transform.position.normalized);
        Destroy(this.gameObject);
      }
      float ratio = (float)lifetime/(maxlife/2f);
      if (ratio < 1f) {
        GetComponentInChildren<SpriteRenderer>().color = origColor * ratio;
        GetComponentInChildren<TextMesh>().color = oCT * ratio;
      }
    }
  }

  public void OnRaycast(int ml) {

    maxlife = ml;
    lifetime = ml;
    GetComponentInChildren<SpriteRenderer>().color = origColor;
    GetComponentInChildren<TextMesh>().color = oCT;
  }

}
