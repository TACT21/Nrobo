using System;
using System.Collections;
using Unity.WebRTC;
using Unity.WebRTC.Samples;
using UnityEngine;
using UnityEngine.UI;

class DataChannelSample : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Button callButton;
    [SerializeField] private Button hangupButton;
    [SerializeField] private Button sendButton;
    [SerializeField] private InputField textSend;
    [SerializeField] private InputField textReceive;
#pragma warning restore 0649

    private RTCPeerConnection local, remote;
    private RTCDataChannel dataChannel, remoteDataChannel;
    private DelegateOnIceConnectionChange localOnIceConnectionChange;
    private DelegateOnIceConnectionChange remoteOnIceConnectionChange;
    private DelegateOnIceCandidate localOnIceCandidate;
    private DelegateOnIceCandidate remoteOnIceCandidate;
    private DelegateOnMessage onDataChannelMessage;
    private DelegateOnOpen onDataChannelOpen;
    private DelegateOnClose onDataChannelClose;
    private DelegateOnDataChannel onDataChannel;

    private void Awake()
    {
        callButton.onClick.AddListener(() => { StartCoroutine(Call()); });
        hangupButton.onClick.AddListener(() => { Hangup(); });

    }

    private void Start()
    {
        callButton.interactable = true;
        hangupButton.interactable = false;

        localOnIceCandidate = candidate => { OnIceCandidate(local, candidate); };
        remoteOnIceCandidate = candidate => { OnIceCandidate(remote, candidate); };
        onDataChannel = channel =>
        {
            remoteDataChannel = channel;
            remoteDataChannel.OnMessage = onDataChannelMessage;
        };
        onDataChannelMessage = bytes => { textReceive.text = System.Text.Encoding.UTF8.GetString(bytes); };
        onDataChannelOpen = () =>
        {
            sendButton.interactable = true;
            hangupButton.interactable = true;
        };
        onDataChannelClose = () =>
        {
            sendButton.interactable = false;
            hangupButton.interactable = false;
        };
    }

    RTCConfiguration GetSelectedSdpSemantics()
    {
        RTCConfiguration config = default;
        config.iceServers = new RTCIceServer[]
        {
            new RTCIceServer { urls = new string[] { "stun:stun.l.google.com:19302" } }
        };

        return config;
    }
    
    void LocalOnIceConnectinChange(RTCIceConnectionState state)
    {
        OnIceConnectionChange(local, state);
    }
    void RemoteOnIceConnectionChange(RTCIceConnectionState state)
    {
        OnIceConnectionChange(remote, state);
    }

    void LocalOnIceCandidate(RTCIceCandidate candidate)
    {
        OnIceCandidate(local, candidate);
    }
    void RemoteOnIceCandidate(RTCIceCandidate candidate)
    {
        OnIceCandidate(remote, candidate);
    }

    IEnumerator Call()
    {
        callButton.interactable = false;
        Debug.Log("GetSelectedSdpSemantics");
        var configuration = GetSelectedSdpSemantics();
        local = new RTCPeerConnection(ref configuration);
        Debug.Log("Created local peer connection object local");
        local.OnIceCandidate = localOnIceCandidate;
        remote = new RTCPeerConnection(ref configuration);
        Debug.Log("Created remote peer connection object remote");
        remote.OnIceCandidate = remoteOnIceCandidate;
        remote.OnDataChannel = onDataChannel;

        RTCDataChannelInit conf = new RTCDataChannelInit();
        dataChannel = local.CreateDataChannel("data", conf);
        dataChannel.OnOpen = onDataChannelOpen;

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

    void Hangup()
    {
        local.Close();
        remote.Close();
        local = null;
        remote = null;

        textSend.text = string.Empty;
        textReceive.text = string.Empty;

        hangupButton.interactable = false;
        sendButton.interactable = false;
        callButton.interactable = true;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="pc"></param>
    /// <param name="streamEvent"></param>
    void OnIceCandidate(RTCPeerConnection pc, RTCIceCandidate candidate)
    {
        GetOtherPc(pc).AddIceCandidate(candidate);
        Debug.Log($"{GetName(pc)} ICE candidate:\n {candidate.Candidate}");
    }

    public void SendMsg()
    {
        dataChannel.Send(textSend.text);
    }
    string GetName(RTCPeerConnection pc)
    {
        return (pc == local) ? "local" : "remote";
    }

    RTCPeerConnection GetOtherPc(RTCPeerConnection pc)
    {
        return (pc == local) ? remote : local;
    }

    IEnumerator OnCreateOfferSuccess(RTCSessionDescription desc)
    {
        Debug.Log($"Offer from local\n{desc.sdp}");
        Debug.Log("local setLocalDescription start");
        var op = local.SetLocalDescription(ref desc);
        yield return op;

        if (!op.IsError)
        {
            OnSetLocalSuccess(local);
        }
        else
        {
            var error = op.Error;
            OnSetSessionDescriptionError(ref error);
        }

        Debug.Log("remote setRemoteDescription start");
        var op2 = remote.SetRemoteDescription(ref desc);
        yield return op2;
        if (!op2.IsError)
        {
            OnSetRemoteSuccess(remote);
        }
        else
        {
            var error = op2.Error;
            OnSetSessionDescriptionError(ref error);
        }
        Debug.Log("remote createAnswer start");
        // Since the 'remote' side has no media stream we need
        // to pass in the right constraints in order for it to
        // accept the incoming offer of audio and video.

        var op3 = remote.CreateAnswer();
        yield return op3;
        if (!op3.IsError)
        {
            yield return OnCreateAnswerSuccess(op3.Desc);
        }
        else
        {
            OnCreateSessionDescriptionError(op3.Error);
        }
    }

    void OnSetLocalSuccess(RTCPeerConnection pc)
    {
        Debug.Log($"{GetName(pc)} SetLocalDescription complete");
    }

    void OnSetSessionDescriptionError(ref RTCError error) { }

    void OnSetRemoteSuccess(RTCPeerConnection pc)
    {
        Debug.Log($"{GetName(pc)} SetRemoteDescription complete"); 
    }

    IEnumerator OnCreateAnswerSuccess(RTCSessionDescription desc)
    {
        Debug.Log($"Answer from remote:\n{desc.sdp}");
        Debug.Log("remote setLocalDescription start");
        var op = remote.SetLocalDescription(ref desc);
        yield return op;

        if (!op.IsError)
        {
            OnSetLocalSuccess(remote);
        }
        else
        {
            var error = op.Error;
            OnSetSessionDescriptionError(ref error);
        }

        Debug.Log("local setRemoteDescription start");

        var op2 = local.SetRemoteDescription(ref desc);
        yield return op2;
        if (!op2.IsError)
        {
            OnSetRemoteSuccess(local);
        }
        else
        {
            var error = op2.Error;
            OnSetSessionDescriptionError(ref error);
        }
    }

    IEnumerator LoopGetStats()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (!sendButton.interactable)
                continue;

            var op1 = local.GetStats();
            var op2 = remote.GetStats();

            yield return op1;
            yield return op2;

            Debug.Log("local");
            foreach (var stat in op1.Value.Stats.Values)
            {
                Debug.Log(stat.Type.ToString());
            }
            Debug.Log("remote");
            foreach (var stat in op2.Value.Stats.Values)
            {
                Debug.Log(stat.Type.ToString());
            }
        }
    }

    void OnAddIceCandidateSuccess(RTCPeerConnection pc)
    {
        Debug.Log($"{GetName(pc)} addIceCandidate success");
    }

    void OnAddIceCandidateError(RTCPeerConnection pc, RTCError error)
    {
        Debug.Log($"{GetName(pc)} failed to add ICE Candidate: ${error}");
    }

    void OnCreateSessionDescriptionError(RTCError e)
    {

    }
}
