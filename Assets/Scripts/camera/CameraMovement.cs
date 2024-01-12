using System;
using UnityEngine;

namespace camera
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform cameraTransform;
        
        public float movementSpeed = 0.2f;
        public float movementTime = 10;
        public Vector3 zoomAmount;

        public Rect bounds = new Rect(-30,-30,60,60);

        [HideInInspector] public Vector3 newPosition;
        [HideInInspector] public Vector3 newZoom;

        private Vector3 dragStartPosition;
        private Vector3 dragCurrentPosition;

        private void Start()
        {
            newPosition = transform.position;
            newZoom = cameraTransform.localPosition;
        }

        private void Update()
        {
            HandleMouse();
            HandleInput();
        }

        private void HandleMouse()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                newZoom += Input.mouseScrollDelta.y * zoomAmount * Time.deltaTime * movementTime;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                var plane = new Plane(Vector3.up, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            
            if (Input.GetMouseButton(0))
            {
                var plane = new Plane(Vector3.up, Vector3.zero);
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float entry;
                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                }
            }
        }

        private void HandleInput()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += transform.forward * (movementSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition += transform.forward * (-movementSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += transform.right * (movementSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition += transform.right * (-movementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Z))
            {
                newZoom += zoomAmount * Time.deltaTime;
            }
            
            if (Input.GetKey(KeyCode.X))
            {
                newZoom -= zoomAmount * Time.deltaTime;
            }

            Vector2 xy = new Vector2(newPosition.x, newPosition.z);
            xy.x = Mathf.Clamp(xy.x, bounds.xMin, bounds.xMax);
            xy.y = Mathf.Clamp(xy.y, bounds.yMin, bounds.yMax);
            newPosition = new Vector3(xy.x, newPosition.y, xy.y);

            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
            cameraTransform.localPosition =
                Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);
        }
    }
}
