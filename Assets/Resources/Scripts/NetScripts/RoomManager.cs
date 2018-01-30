using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using ExitGames.UtilityScripts;
using UnityEngine.SceneManagement;

public class RoomManager : PunBehaviour {
    public List<Button> buttonList;
    public List<Image> Phead;
    public List<Text> Pname;
    public Color[] PlayerColors;
    public Sprite PlayerHead;
    public Sprite[] Scene;
    public Image SceneSprite;
    private int Sc;
    private static int SceneNum = 2;
    private List<string> PhotonName = new List<string>();

    private void Start()
    {
        int Index = PhotonNetwork.player.GetRoomIndex();
        PhotonPlayer player = PhotonNetwork.masterClient;
        for (int i = 0; i <= Index; i++)
        {
            string PlayerName = player.NickName;
            Debug.Log(PlayerName);
            PhotonName.Add(PlayerName);
            Image image = Phead[i].GetComponent<Image>();
            image.sprite = PlayerHead;
            image.color = PlayerColors[i];
            Pname[i].GetComponent<Text>().text = PhotonName[i];
            player = player.GetNext();
        }
        foreach(Button btn in buttonList)
        {
            btn.enabled = false;
        }
    }

    private void Update()
    {
        
    }

    #region Photon Call
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        this.photonView.RPC("SetName", PhotonTargets.All, newPlayer.NickName);
        if(PhotonNetwork.room.PlayerCount == 2 && PhotonNetwork.isMasterClient)
        {
            SetButtonActive();
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("menu");
    }
    #endregion

    #region Private Method
    private void SetButtonActive()
    {
        foreach (Button btn in buttonList)
        {
            btn.enabled = true;
        }
    }
    #endregion

    #region Public Method

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetGameMessage()
    {
        this.photonView.RPC("SetMessage", PhotonTargets.All, null);
    }

    public void nextChoice()
    {
        this.photonView.RPC("SetSceneSprite", PhotonTargets.All, "next");
    }

    public void lastChoice()
    {
        this.photonView.RPC("SetSceneSprite", PhotonTargets.All, "last");
    }

    [PunRPC]
    public void SetSceneSprite(string update)
    {
        if (update == "next")
        {
            Sc++;
            Sc %= SceneNum;
        }
        else
        {
            Sc--;
            Sc = Sc < 0 ? SceneNum - 1 : Sc;
        }
        SceneSprite.GetComponent<Image>().sprite = Scene[Sc];
    }

    [PunRPC]
    public void SetName(string name)
    {
        int index = PhotonName.Count;
        Image image = Phead[index].GetComponent<Image>();
        image.sprite = PlayerHead;
        image.color = PlayerColors[index];
        PhotonName.Add(name);
        Pname[index].GetComponent<Text>().text = PhotonName[index];
    }

    [PunRPC]
    public void SetMessage()
    {
        Sc++;
        if (PhotonNetwork.isMasterClient)
        {
            string SceneName = "NetScene" + Sc;
            PhotonNetwork.LoadLevel(SceneName);
        }
    }
    #endregion
}
