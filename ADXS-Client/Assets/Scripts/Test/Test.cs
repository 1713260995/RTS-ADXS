using Assets.Scripts.Modules.Role;
using UnityEngine;

public class Test : MonoBehaviour
{

    public int n1;
    public int n2 { get; set; }
    public int n3 { get; protected set; }
    private int n4 { get; }
    protected int n5 { get; }
    [HideInInspector]
    public int n6 { get; protected set; }
    public RoleBase role { get; set; }


    [ContextMenu("Print")]
    public void Print()
    {
        print(n1);
        print(n2);
        print(n3);
        print(n4);
        print(n5);
        print(n6);
    }
}
