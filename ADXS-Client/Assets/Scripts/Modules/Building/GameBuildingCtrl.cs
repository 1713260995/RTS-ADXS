using Assets.Scripts.Common.Enum;
using UnityEngine;

namespace Assets.Scripts.Modules.Role
{
    public class GameBuildingCtrl : GameUnitCtrl
    {
        private new Collider collider;
        public BuildingState currentState { get; private set; }

        protected override void Start()
        {
            base.Start();
            collider = GetComponent<Collider>();
        }

        public Vector3 GetBuildingSize() => collider.bounds.size;

        #region Preview

        [SerializeField] private Transform buildPreviewLiner;
        [SerializeField] private Color normalPreviewColor = Color.blue;
        [SerializeField] private Color failedPreviewColor = Color.red;
        [SerializeField] private float intensity = 4.7f;
        private Material previewLinerMaterial;
        private int previewLinerMatId;

        public void SetNormalPreview()
        {
            if (currentState == BuildingState.NormalPreview) return;
            if (previewLinerMaterial == null)
            {
                previewLinerMaterial = buildPreviewLiner.GetComponent<Projector>().material;
                previewLinerMatId = Shader.PropertyToID("_Color");
            }
            currentState = BuildingState.NormalPreview;
            buildPreviewLiner.gameObject.SetActive(true);
            Color hdrColor = normalPreviewColor * intensity;
            previewLinerMaterial.SetColor(previewLinerMatId, hdrColor);
        }

        public void SetFailedPreview()
        {
            if (currentState == BuildingState.FailedPreview) return;
            if (previewLinerMaterial == null)
            {
                previewLinerMaterial = buildPreviewLiner.GetComponent<Projector>().material;
                previewLinerMatId = Shader.PropertyToID("_Color");
            }
            currentState = BuildingState.FailedPreview;
            buildPreviewLiner.gameObject.SetActive(true);
            Color hdrColor = failedPreviewColor * intensity;
            previewLinerMaterial.SetColor(previewLinerMatId, hdrColor);
        }


        public void StartBuild()
        {
            if (currentState == BuildingState.UnderConstruction)
                throw new System.Exception("已经处于建造状态，无法重复执行");
            currentState = BuildingState.UnderConstruction;
            buildPreviewLiner.gameObject.SetActive(false);

        }

        #endregion
    }
}
