//using Assets.GameClientLib.Scripts.Utils.Singleton;
//using System;
//using System.Numerics;
//using UnityEngine;
//using Vector2 = UnityEngine.Vector2;
//using Vector3 = UnityEngine.Vector3;

//namespace Assets.Scripts.Modules.Building
//{
//    internal class BuildManager : SingletonMono<BuildManager>
//    {

//        Terrain _terrain;
//        public static Vector2 cellSize;
//        public static Vector2 gridSize;

//        /**/

//        // 盛放格子的容器
//        private static MapCellNode[,] mapCells;


//        // 初始化格子，并计算每个格子的高度和坡度
//        private void InitMapCells()
//        {
//            var terrainData = _terrain.terrainData;
//            int gridWidth = (int)(terrainData.bounds.size.x / cellSize.x);
//            int gridHeight = (int)(terrainData.bounds.size.z / cellSize.y);

//            mapCells = new MapCellNode[gridWidth, gridHeight];
//            for (int i = 0; i < gridWidth; ++i)
//            {
//                for (int j = 0; j < gridHeight; ++j)
//                {
//                    mapCells[i, j].current = null;

//                    var center = GetCellLocalPosition(i, j);
//                    mapCells[i, j].height = center.y;
//                    var steepness = terrainData.GetSteepness(center.x / terrainData.size.x, center.z / terrainData.size.z);
//                    mapCells[i, j].steepness = steepness;
//                }
//            }
//        }


//        // 根据格子索引，获取格子中心点的本地坐标
//        public static Vector3 GetCellLocalPosition(int w, int h)
//        {
//            Vector3 withoutHeight = new(w * cellSize.x + cellSize.x * 0.5f, 0, h * cellSize.y + cellSize.y * 0.5f);
//            return GetTerrainPosByLocal(withoutHeight);
//        }

//        private static Vector3 GetTerrainPosByLocal(Vector3 withoutHeight)
//        {
//            throw new NotImplementedException();
//        }

//        // 根据格子索引，获取格子中心点的世界坐标
//        public static Vector3 GetCellWorldPosition(int w, int h)
//        {
//            return Instance.transform.TransformPoint(GetCellLocalPosition(w, h));
//        }

//        // 计算地图上的本地坐标点，所属网格的索引
//        public static (int, int) GetCellIndexByLocalPosition(Vector3 local)
//        {
//            return ((int)(local.x / cellSize.x), (int)(local.z / cellSize.y));
//        }

//        // 计算地图上的世界坐标点，所属网格的索引
//        public static (int, int) GetCellIndexByWorldPosition(Vector3 world)
//        {
//            return GetCellIndexByLocalPosition(Instance.transform.InverseTransformPoint(world));
//        }

//        // 根据给定的格子区域（起始格子索引、宽度和高度），计算区域内所有格子的平均高度
//        public static float GetGridAverageHeight(int sx, int sy, int w, int h)
//        {
//            float height = 0;
//            int count = 0;
//            for (int x = sx; x < sx + w; ++x)
//            {
//                if (x < 0 || x >= gridSize.x)
//                    continue;
//                for (int y = sy; y < sy + h; ++y)
//                {
//                    if (y < 0 || y >= gridSize.y)
//                        continue;
//                    height += mapCells[x, y].height;
//                    ++count;
//                }
//            }

//            if (count > 0)
//                return height / count;
//            return 0;
//        }


//        // 开始建造，根据id查询待建物，并持有它。
//        public static void TakeBuilding(string id)
//        {
//            if (!preBuildings.TryGetValue(id, out PreBuilding pb))
//                return;
//            BeginBuild(pb);
//        }

//        // 准备建造指定的建筑物
//        private static void BeginBuild(PreBuilding pb)
//        {
//            // 让待建物准备建造（重置待建物的材质参数等）
//            pb.BeginBuild();
//            currentBuilding = pb;

//            // 在待建物周围绘制方格线
//            Instance.buildLineDrawer.gameObject.SetActive(true);
//            Transform trans = Instance.buildLineDrawer.transform;
//            trans.SetParent(currentBuilding.transform);
//            trans.localPosition = projectorOffset - currentBuilding.AlignToCellOffset();

//            // 如果待建物是具有攻击范围或影响范围的，则显示范围指示器并设置半径为待建物的影响范围
//            if (currentBuilding.canAttack)
//            {
//                Instance.attackCircel.gameObject.SetActive(true);
//                Instance.attackCircel.SetRadius(currentBuilding.AttackRadius);
//                trans = Instance.attackCircel.transform;
//                trans.SetParent(currentBuilding.transform);
//                trans.localPosition = projectorOffset;
//            }
//        }


//        private void Update()
//        {
//            // 不在建造状态就返回
//            if (currentBuilding is null || currentBuilding.IsBuilding)
//            {
//#if DEBUG_MOD
//        DisplayDebugInfo();
//#endif
//                return;
//            }

//            // 按下右键就取消建造
//            if (Input.GetMouseButtonDown(1))
//            {
//                CancelBuild();
//                return;
//            }

//            // 不在UI上才建造
//            if (EventSystem.current.IsPointerOverGameObject())
//            {
//                if (!Cursor.visible)
//                    Cursor.visible = true;

//                return;
//            }

//            // 获取建造点
//            if (!Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100f,
//                    groundLayer.value))
//            {
//                if (!Cursor.visible)
//                    Cursor.visible = true;
//                return;
//            }

//            if (Cursor.visible)
//                Cursor.visible = false;

//            // 按下R键就旋转待建物（换个朝向）
//            if (Input.GetKeyDown(KeyCode.R))
//                currentBuilding.NextRotation();

//            // 获取地图格子索引
//            var (x, y) = GetCellIndexByWorldPosition(hit.point);

//            // 尝试放入待建物，如无法放置返回false
//            if (currentBuilding.CheckBuildingIndexPosOnGrid(x, y))
//            {
//                // 按下左键，准备结束建造
//                if (Input.GetMouseButtonDown(0))
//                {
//                    PrepareEndBuild();
//                }
//            }
//        }


//        public bool CheckBuildingIndexPosOnGrid(int x, int y)
//        {
//            bool canBuild = true;

//            // 根据朝向计算当前占用格子的宽度和高度
//            // 比如：一个建筑物南北朝向放置时占用3*2个格子，但是东西朝向放置时将占用2*3个格子。
//            var (w, h) = GetRealSizeWithDir();

//            int dx = (w - 1) / 2;
//            int dy = (h - 1) / 2;
//            int sx = x - dx;
//            int sy = y - dy;

//            // 获取所占格子的平均地形高度
//            float aheight = BuildManager.GetGridAverageHeight(sx, sy, w, h);

//            string info = "超出范围";

//            for (int px = sx; canBuild && px < sx + w; ++px)
//            {
//                // 判定x方向是否超出地图边界
//                if (px < 0 || px >= BuildManager.gridSize.x)
//                {
//                    canBuild = false;
//                    break;
//                }
//                for (int py = sy; py < sy + h; ++py)
//                {
//                    // 判定z方向是否超出地图边界
//                    if (py < 0 || py >= BuildManager.gridSize.y)
//                    {
//                        canBuild = false;
//                        break;
//                    }

//                    // 判定所占用的格子上是否已经存在其他建筑
//                    if (BuildManager.GetBuildingWithCell(px, py) is not null)
//                    {
//                        canBuild = false;
//                        info = "已存在其他建筑";
//                        break;
//                    }

//                    // 判定格子地形高度与平均高度是否相差太多
//                    if (Mathf.Abs(aheight - BuildManager.GetCellHeight(px, py)) > 0.2f)
//                    {
//                        canBuild = false;
//                        info = "地形不平";
//                        break;
//                    }

//                    // 判定格子坡度是否太陡
//                    if (BuildManager.GetCellStepness(px, py) > 3f)
//                    {
//                        canBuild = false;
//                        info = "坡度太陡";
//                        break;
//                    }
//                }
//            }

//            // 根据格子索引，获取世界坐标，并将其对齐到网格
//            // AlignToCellOffset意义为：假设待建物体的中心点在物体的几何中心，那么，如果所占格子尺寸为奇数，
//            // 则建筑是对称的，偏移为0；如果所占格子尺寸为偶数，则该建筑不是对称的，需要偏移半个单元格。
//            var pos = BuildManager.GetCellWorldPosition(x, y) + AlignToCellOffset();

//            // 如果能够在此处建造，则设置索引，并设置待建物材质为“绿色”，否则设置为“红色”。
//            if (canBuild)
//            {
//                _indexPos.x = sx;
//                _indexPos.y = sy;
//                _indexPos.width = w;
//                _indexPos.height = h;
//                preMaterial.SetColor(CommDefine.PrebuildColor, BuildManager.preBuildNormalColor);
//            }
//            else
//            {
//                preMaterial.SetColor(CommDefine.PrebuildColor, BuildManager.preBuildBadColor);
//                InfoTips.Display(info, pos, 1.2f);
//            }

//            // 设置待建物的世界坐标
//            transform.position = pos;
//            return canBuild;
//        }

//    }
//}
