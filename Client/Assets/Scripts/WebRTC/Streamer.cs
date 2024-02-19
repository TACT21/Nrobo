using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.WebRTC;
using UnityEngine.SocialPlatforms;
using UnityEngine.tvOS;

/// <summary>
/// �P����WebRtc���W���[��
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
            // ��M�����e�N�X�`���� `track.Texture` �v���p�e�B�Ŏ擾�\
        }
        else if (e.Track is AudioStreamTrack track)
        {
            // ���̃g���b�N�̓I�[�f�B�I�p
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
                // ��M�����e�N�X�`���� `track.Texture` �v���p�e�B�Ŏ擾�\
            }
            else if (e.Track is AudioStreamTrack aTrack)
            {
                // ���̃g���b�N�̓I�[�f�B�I�p
            }
        };

        var peerConnection = new RTCPeerConnection();
        peerConnection.OnTrack = (RTCTrackEvent e) => {
            if (e.Track.Kind == TrackKind.Video)
            {
                // ��M�p `MediaStream` �Ƀg���b�N��ǉ�
                // ���̏����� `MediaStream` �� `OnAddTrack` �C�x���g�𔭉΂���
                receiveStream.AddTrack(e.Track);
            }
        };

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
