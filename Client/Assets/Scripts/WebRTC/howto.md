Local/Remote の Candidate 接続に関するコールバックを用意
Local/Remote の RTCPeerConnection オブジェクトを作成
Nat 越えのために RTCConfiguration を上記オブジェクトに設定
Candidate 対応のためのイベント関数を上記オブジェクトの OnIceCandidate に設定
Remote RTCPeerConnection に Channel を設定
RTCDataChannelInit のオブジェクトを Local RTCPeerConnection.CreateDataChannel() に引数で与える
StartCoroutine へ SDPOffer 作成後の IEnumerator を設定
設定された IEnumerator 内で Local RTCPeerConnection.SetLocalDescription を実行。引数は、 SDPOffer を ref で。
設定された IEnumerator 内で Remote RTCPeerConnection.SetLocalDescription を実行。引数は、 SDPOffer を ref で。
設定された IEnumerator 内で Remote RTCPeerConnection.SetLocalDescription を実行。引数は、 SDPAnswer を ref で。
設定された IEnumerator 内で Local RTCPeerConnection.SetLocalDescription を実行。引数は、 SDPAnswer を ref で。
