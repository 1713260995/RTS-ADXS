using UnityEngine;

namespace Assets.Scripts.Modules
{

    public interface ICmd
    {
        bool Execute<T>(T obj);
    }






}
