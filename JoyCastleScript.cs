using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyCastleScript : MonoBehaviour
{
    private enum MoveType
    {
        move,
        easeIn,
        easeOut,
        easeInOut,
    }

    [SerializeField]
    private GameObject go;
    [SerializeField]
    private Vector3 begin = new Vector3(0, 0, -100);
    [SerializeField]
    private Vector3 end = new Vector3(0, 0, 100);
    [SerializeField]
    private float time = 3f;
    [SerializeField]
    private bool pingpong = false;
    [SerializeField]
    private MoveType moveType = MoveType.move;
    void Start()
    {
        switch (moveType)
        {
            case MoveType.move:
                StartCoroutine(move(go, begin, end, time, pingpong));
                break;
            case MoveType.easeIn:
                StartCoroutine(easeIn(go, begin, end, time, pingpong));
                break;
            case MoveType.easeOut:
                StartCoroutine(easeOut(go, begin, end, time, pingpong));
                break;
            case MoveType.easeInOut:
                StartCoroutine(easeInOut(go, begin, end, time, pingpong));
                break;
        }
    }

    IEnumerator move(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
    {
        Vector3 speed = Vector3.zero;
        do
        {
            gameObject.transform.localPosition = begin;
            speed = (end - begin) / time;
            while(gameObject.transform.localPosition != end)
            {
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, end, Vector3.Magnitude(speed) * Time.deltaTime);
                yield return null;
            }
            Vector3 tmpPos = begin;
            begin = end;
            end = tmpPos;
        }
        while (pingpong);
    }

    IEnumerator easeIn(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
    {
        Vector3 speed = Vector3.zero;
        Vector3 addSpeed = Vector3.zero;
        do
        {
            gameObject.transform.localPosition = begin;
            speed = Vector3.zero;
            addSpeed = 2 * (end - begin) / Mathf.Pow(time, 2);//加速度
            while (gameObject.transform.localPosition != end)
            {
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, end, Vector3.Magnitude(speed) * Time.deltaTime);
                speed += addSpeed * Time.deltaTime;
                yield return null;
            }
            Vector3 tmpPos = begin;
            begin = end;
            end = tmpPos;
        }
        while (pingpong);
    }

    IEnumerator easeOut(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
    {
        Vector3 speed = Vector3.zero;
        Vector3 addSpeed = Vector3.zero;
        do
        {
            gameObject.transform.localPosition = begin;
            addSpeed = 2 * (end - begin) / Mathf.Pow(time, 2);//加速度
            speed = addSpeed * time;
            while (gameObject.transform.localPosition != end)
            {
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, end, Vector3.Magnitude(speed) * Time.deltaTime);
                speed -= addSpeed * Time.deltaTime;
                yield return null;
            }
            Vector3 tmpPos = begin;
            begin = end;
            end = tmpPos;
        }
        while (pingpong);
    }

    IEnumerator easeInOut(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong)
    {
        Vector3 speed = Vector3.zero;
        Vector3 addSpeed = Vector3.zero;
        Vector3 tmpDestination = Vector3.zero;
        do
        {
            gameObject.transform.localPosition = begin;
            speed = Vector3.zero;
            tmpDestination = (end - begin) / 2;//起点和终点之间的中点位置作为easeIn的终点
            addSpeed = 2 * (tmpDestination - begin) / Mathf.Pow(time / 2, 2);//加速度
            while (gameObject.transform.localPosition != tmpDestination)
            {
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, tmpDestination, Vector3.Magnitude(speed) * Time.deltaTime);
                speed += addSpeed * Time.deltaTime;
                yield return null;
            }
            while (gameObject.transform.localPosition != end)
            {
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, end, Vector3.Magnitude(speed) * Time.deltaTime);
                speed -= addSpeed * Time.deltaTime;
                yield return null;
            }
            Vector3 tmpPos = begin;
            begin = end;
            end = tmpPos;
        }
        while (pingpong);
    }
}
