using MLAPI;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private float speed;
    
        public override void NetworkStart()
        {
            transform.position = IsOwnedByServer ? 
                new Vector3(-5f, 0f, 0f) :
                new Vector3(5f, 0f, 0f);
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
                return;

            if(Input.GetKey(KeyCode.W))
                transform.position += Vector3.up * (Time.deltaTime * speed);
            if(Input.GetKey(KeyCode.S))
                transform.position -= Vector3.up * (Time.deltaTime * speed);
        }
    }
}
