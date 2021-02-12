using UnityEngine;

namespace MyMarmot.Tools
{
    public static class InputMouse3DCasting
    {
        public static RaycastHit CastRay3D(LayerMask whatToHit)
        {
            RaycastHit Hitten;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out Hitten, 100f, whatToHit);
            return Hitten;
        }

        public static T CheckRaycast3DHittenClass<T>(RaycastHit hit)
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

        public static bool CheckRaycast3DHittenTag(RaycastHit hit, string tagName)
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