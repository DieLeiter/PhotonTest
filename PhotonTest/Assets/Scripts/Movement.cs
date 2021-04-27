using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class Movement : MonoBehaviourPunCallbacks
    {
        private float velocity = 2;


        #region MonoBehavior Callbacks
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 pos = transform.position;

            if(this.photonView.IsMine == true)
            {
                if (Input.GetKey("up"))
                {
                    pos.z += velocity * Time.deltaTime;
                }
                else if (Input.GetKey("down"))
                {
                    pos.z -= velocity * Time.deltaTime;
                }

                if (Input.GetKey("left"))
                {
                    pos.x -= velocity * Time.deltaTime;
                }
                else if (Input.GetKey("right"))
                {
                    pos.x += velocity * Time.deltaTime;
                }

                transform.position = pos;
            }
        }
        #endregion
    }
}

