using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WorldSpaceUIDocument : MonoBehaviour
{
    const string k_TransparentShader = "Unlit/Transparent";
    const string k_TextureShader = "Unlit/Texture";
    const string k_MainTex = "_MainTex";
    static readonly int MainTex = Shader.PropertyToID(k_MainTex);

    [SerializeField] private int PanelWidth = 1280;
    [SerializeField] private int PanelHeight = 720;
    [SerializeField] private float PanelScale = 1.0f;
    [SerializeField] private float PixelsPerUnit = 500.0f;
    [SerializeField] private VisualTreeAsset VisualTreeAsset;
    [SerializeField] private PanelSettings PanelSettingsAsset;
    [SerializeField] private RenderTexture RenderTextureAsset;

    private MeshRenderer m_MeshRenderer;
    private UIDocument m_UIDocument;
    private PanelSettings m_PanelSettings;
    private RenderTexture m_RenderTexture;
    private Material m_Material;

    private void Awake()
    {
        InitializeComponents();
        BuildPanel();
    }

    public void SetLabelText(string label, string text) {
        if (m_UIDocument.rootVisualElement == null) {
            m_UIDocument.visualTreeAsset = VisualTreeAsset;
        }

        // Consider caching the label element for better performace
        m_UIDocument.rootVisualElement.Q<Label>(label).text = text;
    }

    private void BuildPanel()
    {
        CreateRenderTexture();
        CreatePanelSettings();
        CreateUIDocument();
        CreateMaterial();

        SetMaterialToRenderer();
        SetPanelSize();
    }

    private void SetMaterialToRenderer() {
        if (m_MeshRenderer != null) {
            m_MeshRenderer.sharedMaterial = m_Material;
        }
    }

    private void SetPanelSize() {
        if (m_RenderTexture != null && (m_RenderTexture.width != PanelWidth || m_RenderTexture.height != PanelHeight)) {
            m_RenderTexture.Release();
            m_RenderTexture.width = PanelWidth;
            m_RenderTexture.height = PanelHeight;
            m_RenderTexture.Create();

            m_UIDocument?.rootVisualElement?.MarkDirtyRepaint();
        }

        transform.localScale = new Vector3(PanelWidth / PixelsPerUnit, PanelHeight / PixelsPerUnit, 1.0f);
    }

    private void CreateMaterial()
    {
        string shaderName = m_PanelSettings.colorClearValue.a < 1.0f ? k_TransparentShader : k_TextureShader;
        m_Material = new Material(Shader.Find(shaderName));
        m_Material.SetTexture(MainTex, m_RenderTexture);
    }

    private void CreateUIDocument()
    {
        m_UIDocument = gameObject.GetOrAddComponent<UIDocument>();
        m_UIDocument.panelSettings = m_PanelSettings;
        m_UIDocument.visualTreeAsset = VisualTreeAsset;
    }

    private void CreateRenderTexture()
    {
        RenderTextureDescriptor descriptor = RenderTextureAsset.descriptor;
        descriptor.width = PanelWidth;
        descriptor.height = PanelHeight;
        m_RenderTexture = new RenderTexture(descriptor)
        {
            name = $"{name} - RenderTexture",
        };

    }

    private void CreatePanelSettings()
    {
        m_PanelSettings = Instantiate(PanelSettingsAsset);
        m_PanelSettings.targetTexture = m_RenderTexture;
        m_PanelSettings.clearColor = true;
        m_PanelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
        m_PanelSettings.scale = PanelScale;
        m_PanelSettings.name = $"{name} - PanelSettings";
    }

    private void InitializeComponents()
    {
        InitializeMeshRenderer();

        MeshFilter meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        meshFilter.sharedMesh = GetQuadMesh();
    }

    private void InitializeMeshRenderer()
    {
        m_MeshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        m_MeshRenderer.sharedMaterial = null;
        m_MeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        m_MeshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        m_MeshRenderer.receiveShadows = false;
        m_MeshRenderer.lightProbeUsage = LightProbeUsage.Off;
        m_MeshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    }

    static Mesh GetQuadMesh()
    {
        GameObject tempQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Mesh quadMesh = tempQuad.GetComponent<MeshFilter>().sharedMesh;
        Destroy(tempQuad);

        return quadMesh;
    }
}
