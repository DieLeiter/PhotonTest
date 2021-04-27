using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon.StructWrapping;

namespace Com.MyCompany.MyGame
{
    public class CubeManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public fields

        #endregion

        #region Private fields
        [SerializeField]
        private GameObject cube;

        [SerializeField]
        private GameObject sphere;

        private bool isSphere = false;

        private float velocity = 2.0f;
        #endregion

        #region MonoBehaviour callbacks
        // Start is called before the first frame update
        void Start()
        {
            CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();

            if (cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }

            if (photonView.IsMine)
            {
                Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
                photonView.RPC("SetColor", RpcTarget.AllBuffered, color);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                this.ProcessInputs();
            }

            if (isSphere == false)
            {
                cube.SetActive(true);
                sphere.SetActive(false);
            }
            else
            {
                cube.SetActive(false);
                sphere.SetActive(true);
            }
        }
        #endregion

        #region Custom
        void ProcessInputs()
        {
            // Update position
            Vector3 pos = transform.position;

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

            if (Input.GetButtonDown("Jump"))
            {
                isSphere = true;
            }
            if (Input.GetButtonUp("Jump"))
            {
                isSphere = false;
            }
        }

        [PunRPC]
        void SetColor(Color color)
        {
            Material cubeMaterial = cube.GetComponent<Renderer>().material;
            Material sphereMaterial = sphere.GetComponent<Renderer>().material;

            cubeMaterial.SetColor("_Color", color);
            sphereMaterial.SetColor("_Color", color);
        }
        #endregion

        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isSphere);


            }
            else
            {
                // Network player, receive data
                this.isSphere = (bool)stream.ReceiveNext();
            }
        }
        #endregion
    }
}

