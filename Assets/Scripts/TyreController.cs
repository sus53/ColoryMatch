using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TyreController : MonoBehaviour
{
    private Vector3 lastMousePosition;
    private Vector3 initialPosition;
    private float clampedX;
    private float clampedZ;
    public float moveTime;
    private bool isTyreMovable2 = false;
    public GameObject Tyres;
    public GameObject Global;
    public GameObject SecondChanceCanvas;
    public GameObject GameOverCanvas;
    public float dissolveDuration;
    public Material dissolve;
    private List<Material> TyreColor = new List<Material>();
    public AudioSource gameoverAudio;
    public AudioSource backgroundAudio;
    public GameObject TyreHolders;
    public Vector3 GetInitialPosition()
    {
        return this.initialPosition;
    }

    public void SetInitialPosition(Vector3 pos)
    {
        this.initialPosition = pos;
    }
    void Start()
    {
        initialPosition = transform.position;

    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch phase is Began, which is equivalent to mouse down
            if (touch.phase == TouchPhase.Began)
            {
                // Perform a raycast to check if the touch is on this object
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        OnTouchDown();
                    }
                }
            }
        }
    }
    // void OnMouseDown()
    // {
    //     if (!Global.GetComponent<GlobalScript>().GetTyreMovable()) return;
    //     if (!isTyreMovable2) return;
    //     lastMousePosition = Input.mousePosition;
    //     initialPosition = transform.position;
    //     Global.GetComponent<GlobalScript>().SetTyreMovable(false);
    //     StartCoroutine(MoveTyreToCar());
    // }
    void OnTouchDown()
    {
        if (!Global.GetComponent<GlobalScript>().GetTyreMovable()) return;
        if (!isTyreMovable2) return;
        lastMousePosition = Input.mousePosition;
        initialPosition = transform.position;
        Global.GetComponent<GlobalScript>().SetTyreMovable(false);
        StartCoroutine(MoveTyreToCar());
    }
    IEnumerator MoveTyreToCar()
    {
        Vector3 targetPosition = new Vector3(0.005f, 0.144f, 0.68f);
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            float t = elapsedTime / moveTime;

            Vector3 currentPosition = Vector3.Lerp(initialPosition, targetPosition, t);

            float height = Mathf.Sin(t * Mathf.PI * 2.5f) * -0.05f;

            currentPosition.y += height;

            transform.position = currentPosition;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    public void ToggleOnTyreMovable2()
    {
        this.isTyreMovable2 = true;
    }


    public IEnumerator MoveTyreToSpare()
    {
        Material mat0 = GetComponent<Renderer>().materials[0];
        Material mat1 = GetComponent<Renderer>().materials[1];
        mat0.SetFloat("_DissolveStrength", 1f);
        mat1.SetFloat("_DissolveStrength", 1f);
        for (int i = 0; i < transform.childCount; i++)
        {
            Material cmat = transform.GetChild(i).GetComponent<Renderer>().material;
            Color matColor = cmat.color;
            cmat = dissolve;
            cmat.color = matColor;
            cmat.SetFloat("_DissolveStrength", 1f);
        }

        yield return new WaitForSeconds(0.3f);
        GameObject spare = CheckSpare();
        if (spare == null)
        {
            backgroundAudio.Stop();
            gameoverAudio.Play();
            GameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
            yield break;
        }
        if (!spare.activeSelf)
        {
            transform.gameObject.SetActive(false);
            backgroundAudio.Stop();
            gameoverAudio.Play();
            SecondChanceCanvas.SetActive(true);

        }

        transform.position = new Vector3(spare.transform.position.x, 0.144f, spare.transform.position.z);

        float elapsedTime = 0f;
        float duration = 0.8f;
        float dissolveStrength;
        while (elapsedTime < duration)
        {
            dissolveStrength = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            mat0.SetFloat("_DissolveStrength", dissolveStrength);
            mat1.SetFloat("_DissolveStrength", dissolveStrength);
            for (int i = 0; i < transform.childCount; i++)
            {
                Material cmat = transform.GetChild(i).GetComponent<Renderer>().material;
                cmat.SetFloat("_DissolveStrength", dissolveStrength);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.SetParent(spare.transform);
        transform.SetAsFirstSibling();
        Global.GetComponent<GlobalScript>().SetTyreMovable(true);
        if (!spare.activeSelf)
        {
            Time.timeScale = 0f;
        }
    }

    GameObject CheckSpare()
    {
        for (int i = 0; i < TyreHolders.transform.childCount; i++)
        {
            if (TyreHolders.transform.GetChild(i).childCount > 0 && TyreHolders.transform.GetChild(i).transform.GetChild(0).gameObject == transform.gameObject)
            {
                return TyreHolders.transform.GetChild(i).transform.gameObject;
            }
            if (TyreHolders.transform.GetChild(i).transform.childCount == 0)
            {
                return TyreHolders.transform.GetChild(i).transform.gameObject;
            }
        }
        return null;


    }
}
