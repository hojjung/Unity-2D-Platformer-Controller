using UnityEngine;

namespace MyMarmot.Tools
{
    public class Area : MonoBehaviour
    {
        public BoxCollider2D m_Coll2D { get; private set; }
        public Vector2 m_CollTopLeftCorner { get => _boundsTopLeftCorner;private set => _boundsTopLeftCorner = value; }
        public Vector2 m_CollBottomLeftCorner { get => _boundsBottomLeftCorner; private set => _boundsBottomLeftCorner = value; }
        public Vector2 m_CollTopRightCorner { get => _boundsTopRightCorner; private set => _boundsTopRightCorner = value; }
        public Vector2 m_CollBottomRightCorner { get => _boundsBottomRightCorner; private set => _boundsBottomRightCorner = value; }
        public Vector2 m_CollCenter { get => _boundsCenter; private set => _boundsCenter = value; }
        public float m_CollWidth { get => _boundsWidth; private set => _boundsWidth = value; }
        public float m_CollHeight { get => _boundsHeight; private set => _boundsHeight = value; }

        Vector2 _boundsTopLeftCorner;
        Vector2 _boundsBottomLeftCorner;
        Vector2 _boundsTopRightCorner;
        Vector2 _boundsBottomRightCorner;
        Vector2 _boundsCenter;
        float _boundsWidth;
        float _boundsHeight;

        void Awake()
        {
            m_Coll2D = GetComponent<BoxCollider2D>();
            SetAreaCollVectors();
        }


        void SetAreaCollVectors()
        {
            float top = m_Coll2D.offset.y + (m_Coll2D.size.y / 2f);
            float bottom = m_Coll2D.offset.y - (m_Coll2D.size.y / 2f);
            float left = m_Coll2D.offset.x - (m_Coll2D.size.x / 2f);
            float right = m_Coll2D.offset.x + (m_Coll2D.size.x / 2f);

            _boundsTopLeftCorner.x = left;
            _boundsTopLeftCorner.y = top;

            _boundsTopRightCorner.x = right;
            _boundsTopRightCorner.y = top;

            _boundsBottomLeftCorner.x = left;
            _boundsBottomLeftCorner.y = bottom;

            _boundsBottomRightCorner.x = right;
            _boundsBottomRightCorner.y = bottom;

            _boundsTopLeftCorner = transform.TransformPoint(_boundsTopLeftCorner);
            _boundsTopRightCorner = transform.TransformPoint(_boundsTopRightCorner);
            _boundsBottomLeftCorner = transform.TransformPoint(_boundsBottomLeftCorner);
            _boundsBottomRightCorner = transform.TransformPoint(_boundsBottomRightCorner);
            _boundsCenter = m_Coll2D.bounds.center;

            _boundsWidth = Vector2.Distance(_boundsBottomLeftCorner, _boundsBottomRightCorner);
            _boundsHeight = Vector2.Distance(_boundsBottomLeftCorner, _boundsTopLeftCorner);
        }

        public Vector3 GetRandomCalculatedDest()
        {
            float xDest = Random.Range(_boundsBottomLeftCorner.x, _boundsTopRightCorner.x);
            float yDest = Random.Range(_boundsBottomLeftCorner.y, _boundsTopRightCorner.y);

            return new Vector3(xDest, yDest, 0);
        }

        public bool CheckPointContainsInArea(Vector3 targetDest)
        {
            return m_Coll2D.bounds.Contains(targetDest);
        }

    }
}