using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{

    public float moveTime;
    private Vector3 startPosition;
    private Animator animator;
    private Animator characterAnimator;
    public GameObject characterGen;
    private Dictionary<int, List<string>> charData;
    void Start()
    {
        charData = characterGen.GetComponent<CharacterGenerator>().GetCharacterData();
    }

    public bool SearchDeployable(Material carMaterial)
    {
        int row = characterGen.GetComponent<CharacterGenerator>().GetCol();
        Color carColor = carMaterial.color;
        Transform character;
        // int length = 0;
        // if (transform.childCount <= 2)
        // {
        //     length = transform.childCount;
        // }
        // else
        // {
        //     length = Mathf.Clamp(transform.childCount, 0, row);
        // }
        for (int i = 1; i <= charData.Count; i++)
        {
            if (charData[i].Count == 0) continue;
            GameObject go = transform.Find(charData[i][0]).gameObject;

            character = go.transform;
            if (character.GetChild(0).GetComponent<Renderer>().material.color == carColor)
            {
                charData[i].RemoveAt(0);
                animator = character.GetComponent<Animator>();
                StartCoroutine(MoveCharacterToCar(character));
                return true;
            }
        }

        return false;
    }

    IEnumerator MoveCharacterToCar(Transform character)
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isWalk", true);
        float elapsedTime = 0f;
        startPosition = character.transform.position;


        while (elapsedTime < moveTime)
        {
            character.transform.position = Vector3.Lerp(startPosition, new Vector3(0.005f, 0f, 0.751f), elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        int count = 1;

        while (true)
        {
            string moveTurnName = "CharacterClone" + (int.Parse(character.gameObject.name.Substring(14, 2)) + (count * 10));
            if (transform.Find(moveTurnName) != null)
            {
                GameObject ToMoveCharacter = transform.Find(moveTurnName).gameObject;
                CallMoveCharacterToFront(ToMoveCharacter, startPosition);
                startPosition = ToMoveCharacter.transform.position;
                if (count == 1)
                {
                    ToMoveCharacter.transform.SetAsFirstSibling();
                }
            }
            else
            {
                break;
            }

            count++;
        }

        character.transform.position = new Vector3(0.005f, 0.119f, 0.751f);
        Destroy(character.gameObject);
    }
    void CallMoveCharacterToFront(GameObject ToMoveCharacter, Vector3 startPosition)
    {
        StartCoroutine(MoveCharacterToFront(ToMoveCharacter, startPosition));
    }

    IEnumerator MoveCharacterToFront(GameObject character, Vector3 startPosition)
    {
        characterAnimator = character.GetComponent<Animator>();
        characterAnimator.SetBool("isWalk", true);
        yield return new WaitForSeconds(0.2f);
        float elapsedTime = 0f;
        float time = 2f;
        while (elapsedTime < time)
        {
            character.transform.position = Vector3.Lerp(character.transform.position, startPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            if (elapsedTime > (0.7f))
                StopAnimation(character);
            yield return null;
        }
        character.transform.position = startPosition;
    }
    void StopAnimation(GameObject g)
    {
        Animator a = g.GetComponent<Animator>();
        a.SetBool("isWalk", false);
    }



}
