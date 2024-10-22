using UnityEngine;

public class Test : MonoBehaviour
{
    static int count;
    // Start is called before the first frame update
    void Start()
    {
        print("id:" + gameObject.GetInstanceID() + ",count=" + count++);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Destroy(gameObject);
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        // print($"id={gameObject.GetInstanceID()},collision:" + LayerMask.LayerToName(collision.gameObject.layer));
    }


}
