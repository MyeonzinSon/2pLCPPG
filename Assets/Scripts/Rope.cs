using System;
using UnityEngine;

public class Rope : MonoBehaviour
{
    bool stop = true;
    [SerializeField]
    private float rSpeed;
    // [SerializeField]
    // private float startSpeed;
    private float curSpeed;
    [SerializeField]
    private float maxTheta;
    private float curTheta;
    private bool isPlusDir = true;

    [SerializeField]
    private Transform topRopeBar;
    [SerializeField]
    private Transform bottomRopeBar;

    void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        curSpeed = 0;
        curTheta = 0;
        stop = true;
    }
    public void StartMove(Vector2 playerPos)
    {
        if (stop == false) {
            return;
        }
        stop = false;
        Vector2 ropeDir = (topRopeBar.position - bottomRopeBar.position).normalized;
        Vector2 topPos = (Vector2)topRopeBar.position + ropeDir * 0.5f;
        Vector2 bottomPos = (Vector2)bottomRopeBar.position - ropeDir * 0.5f;

        Vector2 newPlayerPoint = NearestPointOnLine(bottomPos, topPos, playerPos);
        bool isLeft = playerPos.x < newPlayerPoint.x;

        curSpeed = FirstSpeed();
        if (isLeft) {
            curSpeed = -curSpeed;
        }
    }

    // 0<= ropePosition <=1
    public Vector2 GetPosFromRatio(float ropePosition)
    {
        Debug.Assert(ropePosition <= 1 && ropePosition >= 0);

        Vector2 ropeDir = (topRopeBar.position - bottomRopeBar.position).normalized;
        Vector2 topPos = (Vector2)topRopeBar.position + ropeDir * 0.5f;
        Vector2 bottomPos = (Vector2)bottomRopeBar.position - ropeDir * 0.5f;

        return Vector2.Lerp(bottomPos, topPos, ropePosition);
    }

    public float GetRatioFromPos(Vector2 playerPos)
    {
        Vector2 ropeDir = (topRopeBar.position - bottomRopeBar.position).normalized;
        Vector2 topPos = (Vector2)topRopeBar.position + ropeDir * 0.5f;
        Vector2 bottomPos = (Vector2)bottomRopeBar.position - ropeDir * 0.5f;

        Vector2 newPlayerPoint = NearestPointOnLine(bottomPos, topPos, playerPos);
        if (bottomPos.x != topPos.x) {
            return Mathf.InverseLerp(bottomPos.x, topPos.x, newPlayerPoint.x);
        } else {
            return Mathf.InverseLerp(bottomPos.y, topPos.y, newPlayerPoint.y);
        }
    }


    void FixedUpdate()
    {
        curSpeed -= rSpeed * Time.fixedDeltaTime * Mathf.Sin(Mathf.Deg2Rad * curTheta);
        curTheta += curSpeed * Time.fixedDeltaTime;

        Vector3 prevAngle = transform.eulerAngles;
        transform.eulerAngles = new Vector3(prevAngle.x, prevAngle.y, curTheta);
    }

    float FirstSpeed()
    {
        float curSpeed = 0;
        float curTheta = maxTheta;

        float dt = 0.1f;
        // 적분을 통해서 똑바로 서있을 때 필요한 속도를 구함
        for (int i=0; i < 1000000; i++) {
            curSpeed -= rSpeed * Time.fixedDeltaTime * Mathf.Sin(Mathf.Deg2Rad * curTheta);
            curTheta += curSpeed * dt;
            if (curTheta < 0) {
                break;
            }
        }

        Debug.Log("Start speed is " + curSpeed);
        return curSpeed;
    }

    private static Vector2 NearestPointOnLine(Vector2 start, Vector2 end, Vector2 point)
    {
        Vector2 lineDir = (end - start).normalized;
        Vector2 start2point = point - start;
        float ratio = Vector2.Dot(start2point, lineDir);
        return start + lineDir * ratio;
    }
}