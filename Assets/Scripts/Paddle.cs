using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float Speed = 2.0f;
    public float MaxMovement = 2.0f;
    private float mouseXLimit = 1.9f;
    private Vector2 lastMousePosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            Vector3 pos = transform.position;
            pos.x += input * Speed * Time.deltaTime;

            if (pos.x > MaxMovement)
                pos.x = MaxMovement;
            else if (pos.x < -MaxMovement)
                pos.x = -MaxMovement;

            transform.position = pos;
        }
        if (Input.mousePresent)
        {
            Vector2 mousePosition = Input.mousePosition;
            if (lastMousePosition != mousePosition)
            {
                lastMousePosition = mousePosition;
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                if (worldPosition.x < -mouseXLimit)
                {
                    worldPosition.x = -mouseXLimit;
                }
                if (worldPosition.x > mouseXLimit)
                {
                    worldPosition.x = mouseXLimit;
                }
                transform.position = new Vector2(worldPosition.x, transform.position.y);
            }
        }
        else if (Input.touchCount > 0)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            if (worldPosition.x < -mouseXLimit)
            {
                worldPosition.x = -mouseXLimit;
            }
            if (worldPosition.x > mouseXLimit)
            {
                worldPosition.x = mouseXLimit;
            }
            transform.position = new Vector2(worldPosition.x, transform.position.y);
        }
    }
}
