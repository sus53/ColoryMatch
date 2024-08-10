using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject Levels;
    public GameObject settings;
    private int level;
    private Vector3 initialPosition;


    void Start()
    {
        level = settings.GetComponent<Settings>().GetLevel();
        ChangeCameraPosition();
        initialPosition = transform.position;
    }

    private void ChangeCameraPosition()
    {
        Transform go = Levels.transform.Find("level " + level);

        transform.position = new Vector3(go.position.x - 18f, transform.position.y, go.position.z - 16f);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        MoveCamera();
                        Debug.Log("Touched");
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        transform.position = initialPosition;
                    }
                }
            }
        }
    }

    void MoveCamera()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 screenPosition = new Vector3(currentMousePosition.x, currentMousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        transform.position = new Vector3(transform.position.x + worldPosition.x, transform.position.y, transform.position.z + worldPosition.z);


    }




}
