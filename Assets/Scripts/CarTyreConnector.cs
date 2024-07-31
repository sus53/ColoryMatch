using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CarTyreConnector : MonoBehaviour
{
    private Material mat;
    private Material tyreChild;
    private GameObject tyre;
    public Transform Characters;
    public GameObject characterGen;
    public float moveTime = 0.7f;
    public Vector3 car_initialPosition;
    public Vector3 car_targetPosition;
    public Vector3 car_finalPosition;
    public VisualEffect smoke;
    public GameObject Global;
    public ParticleSystem smoke1;
    public ParticleSystem smoke2;
    private int characterRow;
    public AudioSource carAudio;
    public AudioSource bubbleAudio;


    void Start()
    {
        if (!mat)
        {
            tyreChild = transform.GetChild(6).GetComponent<Renderer>().material;

            mat = GetComponent<Renderer>().material;
        }
        characterRow = characterGen.GetComponent<CharacterGenerator>().GetRow();
        StartCoroutine(ResetCarPosition(0.2f));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tyre"))
        {
            tyre = other.gameObject;
            Material tyreMat = tyre.GetComponent<Renderer>().material;
            bool isMovable = Characters.GetComponent<MoveCharacter>().SearchDeployable(tyreMat);

            if (isMovable)
            {
                mat.color = tyreMat.color;
                tyreChild.color = tyreMat.color;
                smoke.SetVector4("Color", tyreMat.color);
                smoke.Play();
                bubbleAudio.Play();
                transform.GetComponent<Collider>().enabled = false;
                StartCoroutine(MoveCar());
                StartCoroutine(ResetCarPosition(2.8f));
                Destroy(other.gameObject);
            }
            else
            {
                StartCoroutine(tyre.GetComponent<TyreController>().MoveTyreToSpare());
            }
            StartCoroutine(Global.GetComponent<GlobalScript>().MoveTyreToFront(tyre));
        }
    }
    IEnumerator MoveCar()
    {
        yield return new WaitForSeconds(1.8f);
        carAudio.Play();
        float elapsedTime = 0f;

        smoke1.Play();
        smoke2.Play();
        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(car_targetPosition, car_finalPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = car_finalPosition;

    }

    IEnumerator ResetCarPosition(float time)
    {
        yield return new WaitForSeconds(time);
        carAudio.Play();
        smoke1.Play();
        smoke2.Play();
        mat.color = new Color(1f, 1f, 1f);
        tyreChild.color = new Color(1f, 1f, 1f);
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(car_initialPosition, car_targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = car_targetPosition;
        transform.GetComponent<Collider>().enabled = true;
        Global.GetComponent<GlobalScript>().SetTyreMovable(true);
        if (Characters.childCount == 0)
        {
            Global.GetComponent<GlobalScript>().NextLevel();
        }
        smoke1.Stop();
        smoke2.Stop();
    }

}
