using UnityEngine;
using DG.Tweening;
namespace MyMarmot.Tools
{
    public class CameraFollower : MonoBehaviour
    {
        public Transform PlayerTrans;
        [SerializeField]
        private float _CamMoveSpeed = 5f;
        [SerializeField]
        private float _CamRotSpeed = 50f;
        [SerializeField]
        private float _OffsetX = 3f;

        Transform m_MainCamTrans { get; set; }
        public float m_CamMoveSpeed { get => _CamMoveSpeed; }
        public float m_CamRotSpeed { get => _CamRotSpeed; }
        public float m_OffsetX { get => _OffsetX; }

        Vector3 m_InitMainCamLocalPos { get; set; }


        void Awake()
        {
            m_MainCamTrans = Camera.main.transform;
            m_InitMainCamLocalPos = m_MainCamTrans.localPosition;
        }

        public void CameraFollow(Vector3 Axis, float tickTime)
        {
            TranslateCamera(Axis, tickTime);
            RotateCameraHorizontalAxis(Axis, tickTime);
            MoveMainCamPos(Axis, tickTime);
        }

        private void TranslateCamera(Vector3 Axis, float tickTime)
        {
            transform.position = PlayerTrans.position;
        }

        private void RotateCameraHorizontalAxis(Vector3 Axis, float tickTime)
        {
            transform.RotateAround(PlayerTrans.position, Vector3.up, Axis.x * m_CamRotSpeed * tickTime);
        }

        private void MoveMainCamPos(Vector3 Axis, float tickTime)
        {
            m_MainCamTrans.localPosition = Vector3.Slerp(m_MainCamTrans.localPosition, MoveCameraOffset(Axis), tickTime * m_CamMoveSpeed);
        }

        private Vector3 MoveCameraOffset(Vector3 Axis)
        {
            Vector3 newOffset = m_InitMainCamLocalPos;

            if (Axis.x < 0)
            {
                newOffset.x -= m_OffsetX;
            }
            else if (Axis.x > 0)
            {
                newOffset.x += m_OffsetX;
            }

            return newOffset;
        }

        public void LookForwardChar()
        {
            transform.DORotateQuaternion(PlayerTrans.rotation, 0.5f);
        }
    }
}