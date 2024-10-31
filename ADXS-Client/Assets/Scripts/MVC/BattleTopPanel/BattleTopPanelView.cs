using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Top
{
    public class BattleTopPanelView : MonoBehaviour
    {
        public Button openTaskViewBtn;
        public Button openMenuViewBtn;
        public Button openAllyViewBtn;//盟友视图
        public Button recordVideoBtn;//录制视频

        public Image timeImg;

        public Text goldNumTxt;
        public Text woodNumTxt;
        public Text populationNumTxt;//人口数量

        public BattleTopPanelController uiCtrl;

        public BattleTopPanelView()
        {
            uiCtrl = new BattleTopPanelController(this);
        }
    }
}
