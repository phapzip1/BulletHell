using UnityEngine;

namespace Phac.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private RectTransform BackgroundImage;
        [SerializeField] private RectTransform FillImage;
        [SerializeField] private RectTransform AnimatedImage;
        [SerializeField][Range(0.1f, 3.0f)] private float AnimationSpeed;

        private RectTransform m_RectTransform;
        private float m_Value = 0.0f;
        private float m_EaseValue = 0.0f;
        public float Value
        {
            get => m_Value;
            set
            {
                if (value != m_Value)
                {
                    m_Value = value;
                    OnHealthChanged();
                }
            }
        }

        public float HealthPoint
        {
            get => m_Value;
            set
            {
                if (value != m_Value)
                {
                    m_Value = value;
                    OnHealthChanged();
                }
            }
        }

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            m_Value = 1.0f;
            m_EaseValue = 1.0f;
        }

        private void Update()
        {
            if (m_EaseValue > m_Value)
            {
                m_EaseValue = Mathf.Lerp(m_EaseValue, Value, Time.deltaTime * AnimationSpeed);
                UpdateAnimate();
            }
            else if (m_EaseValue != m_Value)
            {
                m_EaseValue = Value;
                UpdateAnimate();
            }


        }

        public void Reset()
        {
            m_Value = 1.0f;
            m_EaseValue = 1.0f;
            Vector2 parentSize = m_RectTransform.rect.size;
            FillImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentSize.x);
            AnimatedImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentSize.x);
        }

        private void OnHealthChanged()
        {
            // update ui
            Vector2 parentSize = m_RectTransform.rect.size;
            FillImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentSize.x * m_Value);
        }

        private void UpdateAnimate() => AnimatedImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_EaseValue);

    }
}