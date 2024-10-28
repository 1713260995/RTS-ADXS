using Assets.Scripts.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.Top
{
    public class BattleTopPanelController
    {
        public BattleTopPanelView view { get; private set; }
        public GameResource gameResources { get; private set; }

        public BattleTopPanelModel model { get; private set; }


        public BattleTopPanelController(BattleTopPanelView _view)
        {
            view = _view;
            gameResources = new GameResource();
            InitBtnEvent();
            RefreshGameResources();
        }



        public void InitBtnEvent()
        {
            view.openMenuViewBtn.onClick.AddListener(OpenMenuView);
            view.openTaskViewBtn.onClick.AddListener(null);
            view.openAllyViewBtn.onClick.AddListener(null);
            view.recordVideoBtn.onClick.AddListener(RecordVideo);
        }

        private void RecordVideo()
        {
            throw new NotImplementedException();
        }

        private void OpenMenuView()
        {
            throw new NotImplementedException();
        }

        public void RefreshGameResources()
        {
            gameResources.RefreshGameResources();
            view.goldNumTxt.text = gameResources.gold.ToString();
            view.woodNumTxt.text = gameResources.wood.ToString();
            view.populationNumTxt.text = gameResources.population.ToString();
        }

        public void RefreshTimeImg()
        {
            view.timeImg = model.sunImg;
        }
    }
}
