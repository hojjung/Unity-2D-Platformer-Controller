using UnityEngine;

namespace MyMarmot.Tools
{
    public static class InputMouse2DCasting
    {
        public static RaycastHit2D CastRay2D(LayerMask whatToHit)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 100f, whatToHit);

            return hit;
        }

        public static T CheckRaycast2DHittenClass<T>(RaycastHit2D hit)
        {
            if (hit.collider == null)
            {
#if UNITY_EDITOR
                Debug.Log("Hit is Null");
#endif
            }
            else
            {
                var hittenClass = hit.collider.GetComponent<T>();

                if (hittenClass != null)
                {
#if UNITY_EDITOR
                    Debug.Log("Hit:" + hit.collider.name);
#endif
                    return hittenClass;
                }
            }

            T defaultReturn = default(T);
            return defaultReturn;
        }

        public static bool CheckRaycast2DHittenTag(RaycastHit2D hit, string tagName)
        {
            if (hit.collider == null)
            {
#if UNITY_EDITOR
                Debug.Log("Hit is Null");
#endif
                return false;
            }

            if (hit.collider.CompareTag(tagName))
            {
#if UNITY_EDITOR
                Debug.Log("Hit:" + hit.collider.name);
#endif
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}