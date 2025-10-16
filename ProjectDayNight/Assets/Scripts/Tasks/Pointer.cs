using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public Transform pointerTransform;
    private bool isOffScreen;
    private Vector3 targetScreenPoint;
    private float border = 40;
    public float width = 0;
    public float height = 0;
    private SpriteRenderer sr;
    [SerializeField] GameObject targetObject;
    private Color col;
    void Start()
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        if (targetObject==null) target = GameObject.Find("Badguy").transform;
        else target = targetObject.transform;
        pointerTransform = transform;
        sr = GetComponent<SpriteRenderer>();
        col = sr.color;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPos = target.position;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0f;
        Vector3 dir = (targetPos-fromPos).normalized;
        //float angle = Vector3.SignedAngle(dir, new Vector3(1, 0, 0), new Vector3(0, 0, 1));
        var angle = Mathf.Rad2Deg*Mathf.Atan2(dir.y, dir.x);
        targetScreenPoint = Camera.main.WorldToScreenPoint(targetPos);
        isOffScreen = ((targetScreenPoint.x)<=-width || (targetScreenPoint.x)>= (Screen.width+width) || (targetScreenPoint.y)<=-height || (targetScreenPoint.y)>=(Screen.height+height));
        if (isOffScreen) {
            sr.color = new Color(col.r, col.b, col.g, 1f);
            Vector3 capScreenPoint = targetScreenPoint;
            if (capScreenPoint.x <= border) capScreenPoint.x = border;
            if (capScreenPoint.x>=Screen.width-border) capScreenPoint.x = Screen.width-border;
            if (capScreenPoint.y <= border+Screen.height/7) capScreenPoint.y = border+Screen.height/7;
            if (capScreenPoint.y>=Screen.height-border) capScreenPoint.y = Screen.height-border;
            Vector3 pointWorldPos = Camera.main.ScreenToWorldPoint(capScreenPoint);
            pointerTransform.position = pointWorldPos;
 
        } else {
            sr.color = new Color(col.r, col.g, col.b, 0f);
        }
        pointerTransform.rotation = Quaternion.Euler(0, 0, angle -90);

    }
    public void setTarget(GameObject t) {
        targetObject = t;
        target = t.transform;
    }
}
