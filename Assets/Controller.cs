using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Queue<Rigidbody2D> balls;
    public Rigidbody2D ballPrefab;

    public List<Rigidbody2D> activeBalls;
    public GameManager _GameManager;
    private void OnEnable()
    {
        balls = new Queue<Rigidbody2D>();
        for (int i = 0; i < 100; i++)
        {
            Rigidbody2D b = Instantiate(ballPrefab, transform).GetComponent<Rigidbody2D>();
            b.transform.position = new Vector3(100, 100);
            balls.Enqueue(b);
        }
    }

    bool ballShot = false;


    public LineRenderer simulator;
    public Camera cam;

    public Vector2 res;
    private RaycastHit2D ray;
    private void Update()
    {
        Debug.Log(activeBalls.Count);
        int count = activeBalls.Count;
        for (int i = 0; i < count; i++)
        {
            if (activeBalls[i].position.y < -5.15)
            {
                activeBalls[i].velocity = Vector2.zero;
                activeBalls[i].angularVelocity = 0;
                activeBalls[i].gameObject.SetActive(false);
                balls.Enqueue(activeBalls[i]);
                activeBalls.RemoveAt(i);
                count--;
                i--;
            }
        }
        if (count == 0 && !ballShot)
        {
            _GameManager.NextTurn();
            float x = Random.Range(-2f, 2f);
            Rigidbody2D a = balls.Dequeue();
            a.transform.position = new Vector3(x, -4.75f);
            //a.gameObject.SetActive(false);
            activeBalls.Add(a);
        }
        if (!_GameManager.canMove)
            return;




        if (count == 1 /*&& !activeBalls[0].gameObject.activeSelf*/ && ballShot)
        {
            //activeBalls[0].gameObject.SetActive(true);
            ballShot = false;
        }
        else if (count == 1 && !ballShot)
        {
            simulator.transform.position = activeBalls[0].transform.position;

            if (Input.GetMouseButtonUp(0))
            {
                simulator.gameObject.SetActive(false);
                if (Mathf.Abs(res.x) > 0.95 || res.y < 0.3)
                {

                }
                else
                {
                    
                    StartCoroutine(ShootTheBall(activeBalls[0].position.x, res));
                    ballShot = true;
                }

            }
            if (Input.GetMouseButton(0))
            {
                simulator.gameObject.SetActive(true);
                Vector3 a = cam.ScreenToWorldPoint(Input.mousePosition);
                res = (a-activeBalls[0].transform.position).normalized;
                float angle = -Mathf.Atan2(res.x, res.y) * Mathf.Rad2Deg   + 90;
                simulator.transform.rotation = Quaternion.AngleAxis(angle, simulator.transform.forward);

              /*  ray = Physics2D.Raycast(activeBalls[0].transform.position,activeBalls[0].transform.right);
                Debug.DrawRay(activeBalls[0].transform.position,simulator.transform.right*100,Color.red);
               simulator.SetPosition(0,simulator.transform.position);
                simulator.SetPosition(1,ray.point);

                Vector2 poss = Vector2.Reflect(new Vector3(ray.point.x,ray.point.y)-simulator.transform.position,ray.normal);
                simulator.SetPosition(2,ray.point + poss.normalized*2);
                Debug.Log(ray.point + poss.normalized*2);*/
            }

        }
    }

    private float force=1000f;
    IEnumerator ShootTheBall(float startX, Vector2 Direction)
    {
        activeBalls[0].AddForce(Direction * force);
        for (int i = 1; i < _GameManager.level - 1; i++)
        {
            yield return new WaitForSeconds(0.1f);
            Rigidbody2D a = balls.Dequeue();
            a.transform.position = new Vector3(startX, -4.75f);
            activeBalls.Add(a);
            activeBalls[i].AddForce(Direction * force);
        }
    }
}
