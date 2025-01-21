using Assets.Scripts.Modules.AI;
using Assets.Scripts.Modules.Role;
using System;

public interface IBuildAI : IAIBase
{
    GameBuildingCtrl CurrentBuilding { get; }
    void OnBuild(BuildInfo info);
}


public struct BuildInfo
{
    public GameBuildingCtrl building;
    public ExecuteBuildDelegate OnExecuteBuild;

    public delegate void ExecuteBuildDelegate(Action onBuildComplete);

    public BuildInfo(GameBuildingCtrl building, ExecuteBuildDelegate OnExecuteBuild)
    {
        this.building = building;
        this.OnExecuteBuild = OnExecuteBuild;
    }
}

