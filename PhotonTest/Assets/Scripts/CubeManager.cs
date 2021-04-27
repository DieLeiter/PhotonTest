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
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject localPlayerInstance;
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
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                CubeManager.localPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            //CameraWork cameraWork = this.gameObject.GetComponent<CameraWork>();

            /*if (cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }*/

            if (photonView.IsMine)
            {
                Vector3 color = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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
        void SetColor(Vector3 color)
        {
            Material cubeMaterial = cube.GetComponent<Renderer>().material;
            Material sphereMaterial = sphere.GetComponent<Renderer>().material;

            cubeMaterial.SetColor("_Color", new Color(color.x, color.y, color.z, 1));
            sphereMaterial.SetColor("_Color", new Color(color.x, color.y, color.z, 1));
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

