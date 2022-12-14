using UnityEngine;

public class ColorWheel : MonoBehaviour
{
    public RectTransform colorWheel;
    public Texture2D colorTexture;

    public GameObject rightHandAnchor;

    public CanvasGroup mainCanvas;

    private GameObject wall;

    void Update()
    {
        RaycastHit HitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out HitInfo, 100.0f))
        {
            wall = HitInfo.collider.gameObject.tag == "wall" ? HitInfo.collider.gameObject : wall;

            mainCanvas.gameObject.SetActive(true);

            //Vector3 controllerPos = OVRInput.GetLocalControllerPosition(controller);
            //Quaternion controllerRot = OVRInput.GetLocalControllerRotation(controller);

            Vector3 controllerPos = rightHandAnchor.transform.position;
            Quaternion controllerRot = rightHandAnchor.transform.rotation;

            RaycastHit[] objectsHit = Physics.RaycastAll(controllerPos, controllerRot * Vector3.forward);

            foreach (RaycastHit hit in objectsHit)
            {
                if (hit.collider.gameObject.tag == "color")
                {
                    float thisHitDistance = Vector3.Distance(hit.point, controllerPos);
                        
                    Vector3 pos1 = controllerPos + controllerRot * (Vector3.forward * 0.05f);
                    var cursorPosition = controllerPos + controllerRot * (Vector3.forward * thisHitDistance);

                    if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                    {
                        wall.GetComponent<Renderer>().material.color = GetColorFromWheel(cursorPosition);
                    }

                    return;
                }
            }
        }
        else
        {
            mainCanvas.gameObject.SetActive(false);
        }
    }

    public Color GetColorFromWheel(Vector3 cursorPosition)
    {
        var localPos = colorWheel.transform.InverseTransformPoint(cursorPosition);
        var toImg = new Vector2(localPos.x / colorWheel.sizeDelta.x + 0.5f, localPos.y / colorWheel.sizeDelta.y + 0.5f);

        Color sampledColor = Color.black;
        if (toImg.x < 1.0 && toImg.x > 0.0f && toImg.y < 1.0 && toImg.y > 0.0f)
        {
            int Upos = Mathf.RoundToInt(toImg.x * colorTexture.width);
            int Vpos = Mathf.RoundToInt(toImg.y * colorTexture.height);
            sampledColor = colorTexture.GetPixel(Upos, Vpos);
        }
        return new Color(sampledColor.r, sampledColor.g, sampledColor.b, sampledColor.a);
    }
}
