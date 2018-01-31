using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using ExitGames.UtilityScripts;
using UnityEngine.SceneManagement;

public class NetSceneScontroller : PunBehaviour {
    public static NetSceneScontroller instance;
    public Color[] PlayerColors;
    public UIProgressBar HPBar;
    public UIProgressBar MPBar;
    private GameObject localPlayer;
    private static string playerName = "NetPlayer";
    private AudioSource _audioSource;
    // Use this for initialization
    void Start () {
        instance = this;
        localPlayer = (GameObject)Instantiate(Resources.Load(playerName), new Vector3(Random.Range(-17, -15), 0, Random.Range(-3, 3)), Quaternion.identity);
        //GameObject localPlayer = PhotonNetwork.Instantiate(playerName, new Vector3(Random.Range(-11, -9.5f), 0, Random.Range(-2, 2)), Quaternion.identity, 0);
        localPlayer.transform.rotation = Quaternion.Euler(0, 90, 0);
        //int index = PhotonNetwork.player.GetRoomIndex();
        localPlayer.transform.Find("U3DMesh").GetComponent<SkinnedMeshRenderer>().material.color = PlayerColors[0];
        _audioSource = this.gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        HPBar.GetComponent<HpUISlider>().UpdateVal(localPlayer.GetComponent<Role>().hp / 100);
        MPBar.GetComponent<MpUISlider>().UpdateVal(localPlayer.GetComponent<Role>().mp / 100);
    }

    public string getCurrentBulletType()
    {
        return localPlayer.GetComponent<PlayerManager>().getCurrentBulletType();
    }

    public void LeaveRoom()
    {
        StartCoroutine(LeaveDelay());
    }

    public IEnumerator LeaveDelay()
    {
        yield return new WaitForSeconds(3);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("menu");
    }
}
