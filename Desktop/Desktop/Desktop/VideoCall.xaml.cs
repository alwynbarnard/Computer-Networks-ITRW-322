using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Permissions;
using XSocket;

namespace Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class VideoCall : Page
    {
        private string appID = "f87e9078e1ca4c62bfab5f329d893550";
        private string channelName = "nameofchanel1";
        private string token;
        private string userID;
        private string codec = "h264"; //VP8

        // This nested class must be ComVisible for the JavaScript to be able to call it.
        public void showReport(string msg)
        {
            System.Windows.MessageBox.Show(msg);
        }

        [ComVisible(true)]
        public class ScriptManager
        {
            public ScriptManager()
            {

            }
        }

        public VideoCall()
        {
            InitializeComponent();
            userID = Properties.Settings.Default.SignedInID;
            System.Windows.Forms.WebBrowser web = new System.Windows.Forms.WebBrowser();
            //web.ScriptErrorsSuppressed = true;

            // Set the WebBrowser to use an instance of the ScriptManager to handle method calls to C#.
            web.ObjectForScripting = new ScriptManager();

            string temp = Directory.GetCurrentDirectory().Replace("\\", "/");
            // MessageBox.Show(temp);
            string css = temp + "/css/example.css";
            string rtcLatest = temp + "/src/js/XSockets.WebRTC.latest.js";
            string latestJs = temp + "/lib/XSockets.latest.js";
            // Create the webpage
            #region webrtcHtml
            String webrtcHtml = @"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Brood.NET Call</title>
                <meta charset=""utf - 8"" />
                <meta http-equiv=""X - UA - Compatible"" content=""IE = edge""/> 

                        </head>
            <body>    
                       <div class=""localvideo"">
                    <video autoplay></video>
                </div>

                <div class=""remotevideos"">

                </div>
                <ul></ul>

                <div id = ""immediate"" ></div>
                <script src=""" + latestJs + @"""></script>
                <script src =""" + rtcLatest + @""" ></script>
                <script>

                    var $ = function(selector, el)
                    {
                        if (!el) el = document;
                        return el.querySelector(selector);
                    }

                    var trace = function(what, obj) {
                        var pre = document.createElement(""pre"");
                    pre.textContent = JSON.stringify(what) + "" - "" + JSON.stringify(obj || """");
                        $(""#immediate"").appendChild(pre);
                };

                var main = (function() {
                        var broker;
                var rtc;
                var ws = new XSockets.WebSocket(""wss://rtcplaygrouund.azurewebsites.net:443"", [""connectionbroker""], {

                    ctx: '23fbc61c-541a-4c0d-b46e-1a1f6473720a'
                        });

                        var onError = function(err) {
                        };

                        var recordMediaStream = function(stream) {
                            if (""MediaRecorder"" in window === false) {
                                return;
                            }
                            var recorder = new XSockets.MediaRecorder(stream);
            recorder.start();
            recorder.oncompleted = function(blob, blobUrl)
            {
                var li = document.createElement(""li"");
                var download = document.createElement(""a"");
                download.textContent = new Date();
                download.setAttribute(""download"", XSockets.Utils.randomString(8) + "".webm"");
                download.setAttribute(""href"", blobUrl);
                li.appendChild(download);
                                $(""ul"").appendChild(li);

            };
                        };

                        var addRemoteVideo = function(peerId, mediaStream) {
                            var remoteVideo = document.createElement(""video"");
            remoteVideo.setAttribute(""autoplay"", ""autoplay"");
                            remoteVideo.setAttribute(""rel"", peerId);
                            attachMediaStream(remoteVideo, mediaStream);
                            $("".remotevideos"").appendChild(remoteVideo);
                        };
                        var onConnectionLost = function(remotePeer) {
            var peerId = remotePeer.PeerId;
            var videoToRemove = $(""video[rel='"" + peerId + ""']"");
                            $("".remotevideos"").removeChild(videoToRemove);
                        };
                        var oncConnectionCreated = function() {
                            console.log(arguments, rtc);
                        };
                        var onGetUerMedia = function(stream) {
            rtc.connectToContext(); // connect to the current context?
                        };

                        var onRemoteStream = function(remotePeer) {

                            addRemoteVideo(remotePeer.PeerId, remotePeer.stream);
            trace(""Opps, we got a remote stream. lets see if its a goat.."");
                        };
                        var onLocalStream = function(mediaStream) {
            attachMediaStream($("".localvideo video ""), mediaStream);
                            // if user click, video , call the recorder
                            $("".localvideo video "").addEventListener(""click"", function () {
                recordMediaStream(rtc.getLocalStreams()[0]);
            });
                        };
                        var onContextCreated = function(ctx) {
            rtc.getUserMedia(rtc.userMediaConstraints.hd(false), onGetUerMedia, onError);
                        };
                        var onOpen = function() {
            rtc = new XSockets.WebRTC(this);

            rtc.onlocalstream = onLocalStream;
            rtc.oncontextcreated = onContextCreated;
            rtc.onconnectioncreated = oncConnectionCreated;
            rtc.onconnectionlost = onConnectionLost;
            rtc.onremotestream = onRemoteStream;


            rtc.onanswer = function (event) {

            };

            rtc.onoffer = function (event) {

            };

            };



            var onConnected = function() {
            broker = ws.controller(""connectionbroker"");
                            broker.onopen = onOpen;
                        };
                        ws.onconnected = onConnected;
                    });
                    document.addEventListener(""DOMContentLoaded"", main);

                </script>
            </body>
            </html>
            ";
            #endregion
            #region reference
            String HTMLText = @"
                <!DOCTYPE html>
                <html lang=""en"">
                        <head>
                          <meta charset=""UTF-8"">
                          <meta name=""viewport"" content=""width=device-width, initial-scale=1, shrink-to-fit=no"">
                          <title>Bread-Talk</title>
                          <link rel=""stylesheet"" href=""" + Directory.GetCurrentDirectory() + @"/Assets/assets/common.css"" />
                        </head>
                        <body>
                          <script src=""" + Directory.GetCurrentDirectory().Replace("\\", "/") + @"/Assets/vendor/jquery.min.js""></script>
                          <script src=""" + Directory.GetCurrentDirectory().Replace("\\", "/") + @"/Assets/vendor/materialize.min.js""></script>
                          <script src=""" + Directory.GetCurrentDirectory().Replace("\\", "/") + @"/Assets/AgoraRTCSDK-2.9.0.js""></script>
                          <script>
                            videoMain()
                            {
                                window.external.MethodCallFromScript(""Start"");
                                // rtc object
                                var rtc = {
                                  client: null,
                                  joined: false,
                                  published: false,
                                  localStream: null,
                                  remoteStreams: [],
                                  params: {}
                                };

                                // Options for joining a channel
                                var option = {
                                  appID: """ + this.appID + @""",
                                  channel: """ + this.channelName + @""",
                                  uid: """ + this.userID + @""",
                                  token: """ + this.token + @"""
                                };
                                  
                                  // Create a client
                                rtc.client = AgoraRTC.createClient({mode: ""rtc"", codec: """ + this.codec + @"""});

                                // Initialize the client
                                rtc.client.init(option.appID, function() {
                                    window.external.MethodCallFromScript(""init success"");
                                }, (err) => {
                                    window.external.MethodCallFromScript(""Mister Sterling, I have problem: "" + err);
                                });

                                // Join a channel
                                rtc.client.join(option.token ? option.token : null, option.channel, option.uid ? +option.uid : null, function (uid) {
                                    window.external.MethodCallFromScript(""join channel: "" + option.channel + "" success, uid: "" + uid);
                                    rtc.params.uid = uid;
                                        }, function(err)
                                        {
                                            window.external.MethodCallFromScript(""client join failed"", err)
                                   })

                                // Create a local stream
                                rtc.localStream = AgoraRTC.createStream({
                                  streamID: rtc.params.uid,
                                  audio: true,
                                  video: true,
                                  screen: false,
                                })

                                // Initialize the local stream
                                rtc.localStream.init(function () {
                                  window.external.MethodCallFromScript(""init local stream success"");
                                }, function(err)
                                        {
                                            window.external.MethodCallFromScript(""init local stream failed "", err);
                                        })

                                // Publish the local stream
                                rtc.client.publish(rtc.localStream, function (err) {
                                  window.external.MethodCallFromScript(""publish failed"");
                                  window.external.MethodCallFromScript(err);
                                        })

                                //subscribe to stream
                                rtc.client.on(""stream - added"", function (evt) {  
                                  var remoteStream = evt.stream;
                                            var id = remoteStream.getId();
                                            if (id !== rtc.params.uid) {
                                                rtc.client.subscribe(remoteStream, function(err) {
                                                    window.external.MethodCallFromScript(""stream subscribe failed"", err);
                                                })
                                  }
                                            window.external.MethodCallFromScript('stream-added remote-uid: ', id);
                                        });

                                //play stream
                                rtc.client.on(""stream - subscribed"", function (evt) {
                                  var remoteStream = evt.stream;
                                            var id = remoteStream.getId();
                                            // Add a view for the remote stream.
                                            addView(id);
                                            // Play the remote stream.
                                            remoteStream.play(""remote_video_"" + id);
                                            window.external.MethodCallFromScript('stream-subscribed remote-uid: ', id);
                                        })

                                //remove stream when closed
                                rtc.client.on(""stream - removed"", function (evt) {
                                  var remoteStream = evt.stream;
                                            var id = remoteStream.getId();
                                            // Stop playing the remote stream.
                                            remoteStream.stop(""remote_video_"" + id);
                                            // Remove the view of the remote stream. 
                                            removeView(id);
                                            window.external.MethodCallFromScript('stream-removed remote-uid: ', id);
                                        })
                            }

                            
                            window.onload = videoMain();

                            // Leave the channel
                            rtc.client.leave(function () {
                              // Stop playing the local stream
                              rtc.localStream.stop();
                              // Close the local stream
                              rtc.localStream.close();
                              // Stop playing the remote streams and remove the views
                              while (rtc.remoteStreams.length > 0) {
                                var stream = rtc.remoteStreams.shift();
                                var id = stream.getId();
                                stream.stop();
                                removeView(id);
                              }
                              window.external.MethodCallFromScript(""client leaves channel success"");
                            }, function(err)
                                    {
                                        window.external.MethodCallFromScript(""channel leave failed"");
                                        window.external.MethodCallFromScript(err);
                                    })
                          </script>
                        </body>
                </html>
            ";
            #endregion

            DirectoryInfo di = new DirectoryInfo($@"{AppDomain.CurrentDomain.BaseDirectory}\Calling");
            if (!di.Exists)
                di.Create();

            string filename = $@"{AppDomain.CurrentDomain.BaseDirectory}Calling\temp.html";
            File.WriteAllText(filename, webrtcHtml);

            web.ObjectForScripting = filename;
            Process.Start(filename);
            //
            //web.Navigate(filename, null, null, "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
        }
    }
}

