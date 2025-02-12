using System.Collections.Generic;
using Assets.GameClientLib.Scripts.Utils;
using Assets.Scripts.Common.Enum;
using Modules.Rendering;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    /// <summary>
    /// 树木
    /// </summary>
    public class TreeCtrl : GameUnitCtrl
    {
        public List<SkinnedMeshRenderer> meshRenderers;

        public GameObject TreeModel;

        // void OnBecameVisible()
        // {
        //     print($"{gameObject.name}:{id} is Visible");
        //     TreeModel.SetActive(true);
        // }
        //
        // void OnBecameInvisible()
        // {
        //     print($"{gameObject.name}:{id} is Invisible");
        //     TreeModel.SetActive(false);
        // }
    }
}