using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;
using UnityEngine.SocialPlatforms;
using UnityEngine.tvOS;

/// <summary>
/// 単方向WebRtcモジュール
/// </summary>
public class Streamer : MonoBehaviour
{
    private RTCSessionDescription sessionDescription;
    private RTCPeerConnection remotePeerConnection;
    private RTCPeerConnection localPeerConnection;
    private string Answer;
    private bool IsInitialized = false;
    public string Sdp { set; get; } = "";
    public string[] RTCIceServers { set; get; } = new string[] { "stun:stun.l.google.com:19302" }();

    public void Enable ()
    {
        if(!IsInitialized && Sdp != "")
        {
            Call();
        }
    }

    public RTCPeerConnection GetOtherConection (RTCPeerConnection target)
    {
        return target.Equals(localPeerConnection) ? remotePeerConnection :localPeerConnection ;
    }

    RTCConfiguration GetSelectedSdpSemantics()
    {
        RTCConfiguration config = default;
        config.iceServers = new RTCIceServer[]
        {
            new RTCIceServer { urls = RTCIceServers }
        };
        return config;
    }

    private IEnumerator Call()
    {
        //Create configuration
        var configuration = GetSelectedSdpSemantics();

        //Create localPeerConnection
        localPeerConnection = new RTCPeerConnection(ref configuration);
        localPeerConnection.OnIceCandidate = candidate => {remotePeerConnection.AddIceCandidate(candidate);};

        //Create remotePeerConnection
        remotePeerConnection = new RTCPeerConnection(ref configuration);
        remotePeerConnection.OnIceCandidate = candidate => { localPeerConnection.AddIceCandidate(candidate); };

        //Create & Attach MediaStream
        var receiveStream = new MediaStream();
        receiveStream.OnAddTrack = OnAddTrack;
        remotePeerConnection.OnTrack = (RTCTrackEvent e) => {
            if (e.Track.Kind == TrackKind.Video)
            {
                receiveStream.AddTrack(e.Track);
            }
        };

        //Create Offer Sdp
        var op = localPeerConnection.CreateOffer();
        yield return op;

        Debug.Log("local createOffer start");
        var op = local.CreateOffer();
        yield return op;

        if (!op.IsError)
        {
            yield return StartCoroutine(OnCreateOfferSuccess(op.Desc));
        }
        else
        {
            OnCreateSessionDescriptionError(op.Error);
        }
    }

    private void OnAddTrack (MediaStreamTrackEvent e)
    {
        if (e.Track is VideoStreamTrack track)
        {
            // 受信したテクスチャは `track.Texture` プロパティで取得可能
        }
        else if (e.Track is AudioStreamTrack track)
        {
            // このトラックはオーディオ用
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var receiveStream = new MediaStream();
        receiveStream.OnAddTrack = e =>
        {
            if (e.Track is VideoStreamTrack vTrack)
            {
                // 受信したテクスチャは `track.Texture` プロパティで取得可能
            }
            else if (e.Track is AudioStreamTrack aTrack)
            {
                // このトラックはオーディオ用
            }
        };

        var peerConnection = new RTCPeerConnection();
        peerConnection.OnTrack = (RTCTrackEvent e) => {
            if (e.Track.Kind == TrackKind.Video)
            {
                // 受信用 `MediaStream` にトラックを追加
                // この処理は `MediaStream` の `OnAddTrack` イベントを発火する
                receiveStream.AddTrack(e.Track);
            }
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
