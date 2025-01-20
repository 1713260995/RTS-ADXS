using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Test.UnitTest
{
    public class UnitTest_Base : MonoBehaviour
    {

        protected virtual void Start()
        {
            CheckRequireModule();
        }

        /// <summary>
        /// 检查单元测试必需的组件是否存在
        /// </summary>
        protected virtual void CheckRequireModule() { }
    }
}

