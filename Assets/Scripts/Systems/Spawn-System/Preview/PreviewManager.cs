using UnityEngine;

namespace Game.Spawning.Preview
{
    public sealed class PreviewManager
    {
        private GameObject previewInstance;
        private Material previewMaterial;
        public Quaternion CurrentRotation { get; private set; }

        public void CreatePreview(GameObject previewPrefab)
        {
            DestroyPreview();
            previewInstance = Object.Instantiate(previewPrefab);
            previewMaterial = CreatePreviewMaterial();
            ApplyMaterialToPreview();
        }

        public void UpdatePreview(Vector3 position, bool isValid)
        {
            if (!previewInstance) return;
            
            previewInstance.transform.position = position;
            previewMaterial.color = isValid 
                ? new Color(0, 1, 0, 0.5f) 
                : new Color(1, 0, 0, 0.5f);
        }

        public void Rotate(float degrees)
        {
            CurrentRotation *= Quaternion.Euler(0, degrees, 0);
            if (previewInstance)
                previewInstance.transform.rotation = CurrentRotation;
        }

        public void DestroyPreview()
        {
            if (previewInstance)
                Object.Destroy(previewInstance);
            
            CurrentRotation = Quaternion.identity;
        }

        private Material CreatePreviewMaterial()
        {
            return new Material(Shader.Find("Standard"))
            {
                color = new Color(0, 1, 0, 0.5f),
                renderQueue = 3000
            };
        }

        private void ApplyMaterialToPreview()
        {
            foreach (var renderer in previewInstance.GetComponentsInChildren<Renderer>())
                renderer.material = previewMaterial;
        }
    }
}