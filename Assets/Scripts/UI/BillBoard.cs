using UnityEngine;

namespace Phac.UI
{
    public class BillBoard : MonoBehaviour
    {
        private void LateUpdate() => transform.LookAt(Camera.main.transform.position);
    }
}