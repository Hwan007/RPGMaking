using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ProjectCode;

namespace ProjectCode
{
    public class CameraRay : MonoBehaviour
    {
        public Camera cameraRay;
        Ray ray;
        public RaycastHit hit;
        string down, up;

        // arrow below part name object (head/arrow , ...)
        GameObject selectedObjectArrow;

        // part name object (head, torso, right arm, ... etc)
        // null as default.
        public GameObject SelectedPart;

        EventSystem eventSystem;


        // Start is called before the first frame update
        void Start()
        {
            cameraRay = GetComponent<Camera>();
            eventSystem = FindObjectOfType<EventSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider check;
                bool UI;
                CheckObjectOnMousePosition(out check, out UI);
                if (UI)
                {
                    Debug.Log("..");
                }
                else if (check == null)
                {
                    down = null;
                }
                else
                {
                    // check = background.
                    // head(part name) = background.pareant.
                    down = check.transform.parent.name;
                    Debug.Log("Mouse Btn Down : " + down);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Collider check;
                bool UI;
                CheckObjectOnMousePosition(out check, out UI);
                if (UI)
                {
                    Debug.Log("..");
                }
                else if (check == null)
                {
                    up = null;
                    if (selectedObjectArrow != null)
                    {
                        if (selectedObjectArrow.activeInHierarchy)
                            selectedObjectArrow.SetActive(false);
                        selectedObjectArrow = null;
                    }
                }
                else
                {
                    // check = background.
                    // head(part name) = background.parent.
                    up = check.transform.parent.name;
                    if (down == null)
                    {
                        up = null;
                        SelectedPart = null;
                        if (selectedObjectArrow != null)
                        {
                            if (selectedObjectArrow.activeInHierarchy)
                                selectedObjectArrow.SetActive(false);
                            selectedObjectArrow = null;
                        }
                    }
                    else
                    {
                        if (up == down)
                        {
                            SelectedPart = check.transform.parent.gameObject;
                            Transform arrow;
                            arrow = check.transform.parent.Find("arrow");
                            if (selectedObjectArrow == null)
                            {
                                if (arrow == null)
                                {
                                    Debug.Log("UI or Parts not selected");
                                }
                                else
                                {
                                    selectedObjectArrow = arrow.gameObject;
                                    selectedObjectArrow.SetActive(true);
                                }
                            }
                            else
                            {
                                if (selectedObjectArrow.activeInHierarchy)
                                    selectedObjectArrow.SetActive(false);

                                if (arrow == null)
                                {
                                    Debug.Log("UI or Parts not selected");
                                }
                                else
                                {
                                    selectedObjectArrow = arrow.gameObject;
                                    selectedObjectArrow.SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            up = null;
                            SelectedPart = null;
                            if (selectedObjectArrow != null)
                            {
                                if (selectedObjectArrow.activeInHierarchy)
                                    selectedObjectArrow.SetActive(false);
                                selectedObjectArrow = null;
                            }
                        }
                    }
                }
            }
        }

        public bool CheckObjectOnMousePosition(out Collider collider, out bool UICheck)
        {
            // return 'Collider' if same Objects on mouse position when clicked down&up
            bool collide = false;

            // for check UI
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);

            if (results.Count > 0)
            {
                collide = true;
                GameObject hitObject1 = results[0].gameObject;
                if (results.Count > 1)
                {
                    UICheck = true;
                    /*
                    for (int n = 0; n < results.Count; n++)
                        Debug.Log("No." + n + " mouse ray hit object : " + results[n].gameObject.name);
                    */
                }

                // If UI Image is filled than 'hitImgae' isn't null.
                Image hitImage = hitObject1.GetComponent<Image>();
                if (hitImage == null)
                {
                    UICheck = false;
                    // check object using camera ray
                    ray = cameraRay.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        collider = hit.collider;
                    }
                    else
                    {
                        collider = null;
                    }
                }
                else
                {
                    UICheck = true;
                    collider = null;
                }
            }
            else
            {
                // UI is not detected.
                UICheck = false;
                // check object using camera ray.
                ray = cameraRay.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    collide = true;
                    collider = hit.collider;
                }
                else
                {
                    collider = null;
                }
            }
            return collide;
        }
    }
}