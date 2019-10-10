using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System.Linq;
using UnityEngine.SceneManagement;

public class EndResult : AbstractRoomState
{
    [SerializeField]
    RoomUnit prefab;

    [SerializeField]
    Transform trsf;

    List<RoomUnit> allRoomUnit = new List<RoomUnit>();

    public override void Init()
    {
        base.Init();
        foreach(Player plyr in PhotonNetwork.CurrentRoom.Players.Values)
        {
            RoomUnit ru = Instantiate(prefab, trsf);
            ru.SetText(plyr.NickName + " : " + plyr.CustomProperties["Score"]);
            allRoomUnit.Add(ru);
        }
        StartCoroutine(Restart());
    }

    public override void Uninit()
    {
        base.Uninit();
        foreach(RoomUnit ru in allRoomUnit)
        {
            Destroy(ru.gameObject);
        }
        allRoomUnit.Clear();
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
