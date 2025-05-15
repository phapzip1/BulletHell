using UnityEngine;

namespace Phac.Scriptable
{
    [CreateAssetMenu(fileName = "Bullet Config", menuName = "ScriptableObject/Bullet Config")]
    public class BulletConfig : ScriptableObject
    {
        #region Rendering Attributes
        public AnimationCurve WidthCurve;
        public float Time = 0.5f;
        public float MinVertexDistance = 0.1f;
        public Gradient ColorGradient;
        public Material Material;
        public int CornerVertices;
        public int EndCapVertices;
        #endregion

        #region Physics Attribute
        public float Speed;
        public float LifeTime;
        #endregion

        public void SetupTrail(TrailRenderer trailRenderer)
        {
            trailRenderer.widthCurve = WidthCurve;
            trailRenderer.time = Time;
            trailRenderer.sharedMaterial = Material;
            trailRenderer.minVertexDistance = MinVertexDistance;
            trailRenderer.colorGradient = ColorGradient;
            trailRenderer.numCornerVertices = CornerVertices;
            trailRenderer.numCapVertices = EndCapVertices;
            
        }
    }
}