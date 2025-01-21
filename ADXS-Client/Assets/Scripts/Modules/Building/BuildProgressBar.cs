using Assets.GameClientLib.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class BuildProgressBar : MonoBehaviour
{
    [SerializeField] private Image buildProgressImg;

    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.forward);
    }

    public void SetBuildProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);
        buildProgressImg.fillAmount = progress;
    }
}
