using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    public bool rotate = true;
    private GameMaster gameMaster;
    private Camera gameCam;
    private Vector3 camStartPos = new Vector3(0, 15, -7);

    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        Debug.Assert(gameMaster != null, "Camera script did not find GameMaster!");

        gameCam = FindObjectOfType<Camera>();
        Debug.Assert(gameCam != null, "Camera script did not find Camera in scene!");
    }
    
    void LateUpdate()
    {
        if (rotate)
        {
            gameCam.transform.LookAt(CalculateBestMiddle());
        }
        else
        {
            // calculate greatest distance between all players and change camera position/lookAt accordingly through lerp
            // note: lerp values based on experimentation, need to be edited if level size should ever change
            Vector3 greatestDistance = CalculateGreatestPlayerDistance();
            camStartPos.y = Mathf.Lerp(6, 17, greatestDistance.magnitude / 45f);
            camStartPos.z = Mathf.Lerp(-7, -17, greatestDistance.magnitude / 50f);
            gameCam.transform.position = CalculateBestMiddle() + camStartPos;
            gameCam.transform.LookAt(CalculateBestMiddle());
        }
    }

    private Vector3 CalculateBestMiddle()
    {
        // how to calculate a centroid: https://math.stackexchange.com/questions/1599095/how-to-find-the-equidistant-middle-point-for-3-points-on-an-arbitrary-polygon
        
        float middleX = 0f;
        float middleY = 0f;
        float middleZ = 0f;
        if (gameMaster != null
            && gameMaster.GetCurrentPlayers() != null)
        {
            foreach(HealthSystem player in gameMaster.GetCurrentPlayers())
            {
                middleX += player.transform.position.x;
                middleY += player.transform.position.y;
                middleZ += player.transform.position.z;
            }

            middleX = middleX / gameMaster.GetCurrentPlayers().Count;
            middleY = middleY / gameMaster.GetCurrentPlayers().Count;
            middleZ = middleZ / gameMaster.GetCurrentPlayers().Count;
        }

        return new Vector3(middleX, middleY, middleZ);
    }

    private Vector3 CalculateGreatestPlayerDistance()
    {
        float greatestXDistance = 0f;
        float greatestZDistance = 0f;
        if (gameMaster != null
        && gameMaster.GetCurrentPlayers() != null)
        {
            foreach (HealthSystem p1 in gameMaster.GetCurrentPlayers())
            {
                foreach (HealthSystem p2 in gameMaster.GetCurrentPlayers())
                {
                    float distX = Mathf.Abs(p1.transform.position.x - p2.transform.position.x);
                    float distZ = Mathf.Abs(p1.transform.position.z - p2.transform.position.z);
                    if (distX > greatestXDistance)
                    {
                        greatestXDistance = distX;
                    }
                    if (distZ > greatestZDistance)
                    {
                        greatestZDistance = distZ;
                    }
                }
            }
        }
        return new Vector3(greatestXDistance, 0, greatestZDistance);
    }
}
