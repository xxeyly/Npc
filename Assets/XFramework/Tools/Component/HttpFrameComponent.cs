using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace XFramework
{
    public class HttpFrameComponent : FrameComponent
    {
        public static HttpFrameComponent Instance;
        private UnityWebRequest _request;
        [LabelText("是否联网")] public bool notReachable;

        [DllImport("winInet.dll")] //引用外部库
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved); //库中函数

        /// <summary>
        /// Http请求模式
        /// </summary>
        public enum HttpRequestMethod
        {
            GET,
            PUT,
            DELETE,
            POST
        }

        private void Update()
        {
            notReachable = IsConnected();
        }


        /// <summary>
        /// 判断连接状态
        /// </summary>
        private bool IsConnected()
        {
            int dwFlag = new int();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                if ((dwFlag & 0x14) == 0)
                {
                    return false;
                }
            }
            else
            {
                if ((dwFlag & 0x01) != 0)
                {
                    return true;
                }
                else if ((dwFlag & 0x02) != 0)
                {
                    return true;
                }
                else if ((dwFlag & 0x04) != 0)
                {
                    return true;
                }
                else if ((dwFlag & 0x40) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override void FrameInitComponent()
        {
            Instance = GetComponent<HttpFrameComponent>();
        }

        public override void FrameSceneInitComponent()
        {
        }

        public override void FrameEndComponent()
        {
            Instance = null;
        }


        /// <summary>
        /// 发送Http请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="requestMethod">请求方式</param>
        /// <param name="action">返回数据执行事件</param>
        /// <param name="requestData">请求数据</param>
        public void SendHttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action,
            string requestData = "")
        {
            StartCoroutine(UnityHttpWebRequest(url, requestMethod, action, requestData));
        }

        [Obsolete("Obsolete")]
        public void SendHttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction)
        {
            StartCoroutine(HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction));
        }

        [Obsolete("Obsolete")]
        public void SendHttpUnityWebRequest<T>(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string, T> action, Action<string> errorAction, T t)
        {
            StartCoroutine(HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction, t));
        }

        [Obsolete("Obsolete")]
        public void SendHttpUnityWebRequest<T1, T2>(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string, T1, T2> action, Action<string> errorAction, T1 t1, T2 t2)
        {
            StartCoroutine(HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction, t1, t2));
        }

        [Obsolete("Obsolete")]
        public void SendHttpUnityWebRequest<T1, T2, T3>(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string, T1, T2, T3> action, Action<string> errorAction, T1 t1, T2 t2, T3 t3)
        {
            StartCoroutine(HttpUnityWebRequest(url, requestMethod, requestData, action, errorAction, t1, t2, t3));
        }

        [Obsolete("Obsolete")]
        IEnumerator HttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string> action, Action<string> errorAction)
        {
            UnityWebRequest webRequest = null;
            switch (requestMethod)
            {
                case HttpRequestMethod.GET:
                    webRequest = UnityWebRequest.Get(url + DictionaryToString(requestData));
                    break;
                case HttpRequestMethod.PUT:
                    break;
                case HttpRequestMethod.DELETE:
                    break;
                case HttpRequestMethod.POST:
                    WWWForm wwwForm = new WWWForm();
                    foreach (KeyValuePair<string, string> pair in requestData)
                    {
                        wwwForm.AddField(Regex.Unescape(pair.Key), Regex.Unescape(pair.Value));
                    }

                    webRequest = UnityWebRequest.Post(url, wwwForm);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("requestMethod", requestMethod, null);
            }

            if (webRequest != null)
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    errorAction.Invoke(webRequest.error);
                }
                else
                {
                    action.Invoke(Regex.Unescape(webRequest.downloadHandler.text));
                }

                webRequest.Dispose();
            }
        }

        [Obsolete("Obsolete")]
        IEnumerator HttpUnityWebRequest<T>(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string, T> action, Action<string> errorAction, T t)
        {
            UnityWebRequest webRequest = null;
            switch (requestMethod)
            {
                case HttpRequestMethod.GET:
                    webRequest = UnityWebRequest.Get(url + DictionaryToString(requestData));
                    break;
                case HttpRequestMethod.PUT:
                    break;
                case HttpRequestMethod.DELETE:
                    break;
                case HttpRequestMethod.POST:
                    WWWForm wwwForm = new WWWForm();
                    foreach (KeyValuePair<string, string> pair in requestData)
                    {
                        wwwForm.AddField(Regex.Unescape(pair.Key), Regex.Unescape(pair.Value));
                    }

                    webRequest = UnityWebRequest.Post(url, wwwForm);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("requestMethod", requestMethod, null);
            }

            if (webRequest != null)
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.url + "访问错误");
                    errorAction.Invoke(webRequest.url + "访问错误");
                }
                else
                {
                    action.Invoke(Regex.Unescape(webRequest.downloadHandler.text), t);
                }

                webRequest.Dispose();
            }
        }


        [Obsolete("Obsolete")]
        IEnumerator HttpUnityWebRequest<T1, T2>(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string, T1, T2> action, Action<string> errorAction, T1 t1, T2 t2)
        {
            UnityWebRequest webRequest = null;
            switch (requestMethod)
            {
                case HttpRequestMethod.GET:
                    webRequest = UnityWebRequest.Get(url + DictionaryToString(requestData));
                    break;
                case HttpRequestMethod.PUT:
                    break;
                case HttpRequestMethod.DELETE:
                    break;
                case HttpRequestMethod.POST:
                    WWWForm wwwForm = new WWWForm();
                    foreach (KeyValuePair<string, string> pair in requestData)
                    {
                        wwwForm.AddField(Regex.Unescape(pair.Key), Regex.Unescape(pair.Value));
                    }

                    webRequest = UnityWebRequest.Post(url, wwwForm);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("requestMethod", requestMethod, null);
            }

            if (webRequest != null)
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.url + "访问错误");
                    errorAction.Invoke(webRequest.url + "访问错误");
                }
                else
                {
                    action.Invoke(Regex.Unescape(webRequest.downloadHandler.text), t1, t2);
                }
            }
        }

        [Obsolete("Obsolete")]
        IEnumerator HttpUnityWebRequest<T1, T2, T3>(string url, HttpRequestMethod requestMethod, Dictionary<string, string> requestData, Action<string, T1, T2, T3> action, Action<string> errorAction, T1 t1, T2 t2, T3 t3)
        {
            UnityWebRequest webRequest = null;
            switch (requestMethod)
            {
                case HttpRequestMethod.GET:
                    webRequest = UnityWebRequest.Get(url + DictionaryToString(requestData));
                    break;
                case HttpRequestMethod.PUT:
                    break;
                case HttpRequestMethod.DELETE:
                    break;
                case HttpRequestMethod.POST:
                    WWWForm wwwForm = new WWWForm();
                    foreach (KeyValuePair<string, string> pair in requestData)
                    {
                        wwwForm.AddField(Regex.Unescape(pair.Key), Regex.Unescape(pair.Value));
                    }

                    webRequest = UnityWebRequest.Post(url, wwwForm);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("requestMethod", requestMethod, null);
            }

            if (webRequest != null)
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isHttpError || webRequest.isNetworkError)
                {
                    Debug.Log(webRequest.url + "访问错误");
                    errorAction.Invoke(webRequest.url + "访问错误");
                }
                else
                {
                    action.Invoke(Regex.Unescape(webRequest.downloadHandler.text), t1, t2, t3);
                }
            }
        }


        private string DictionaryToString(Dictionary<string, string> parameter)
        {
            string content = String.Empty;
            foreach (KeyValuePair<string, string> pair in parameter)
            {
                if (content == string.Empty)
                {
                    content += ("?");
                }
                else
                {
                    content += ("&");
                }

                content += pair.Key + "=" + pair.Value;
            }


            return Regex.Unescape(content);
        }


        IEnumerator UnityHttpWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action, string requestData = "")
        {
            if (requestData.Length == 0)
            {
                requestData += requestMethod;
            }

            byte[] databyte = Encoding.UTF8.GetBytes(requestData);
            _request = new UnityWebRequest(url, requestMethod.ToString());
            _request.uploadHandler = new UploadHandlerRaw(databyte);
            _request.downloadHandler = new DownloadHandlerBuffer();
            _request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            yield return _request.SendWebRequest();

#pragma warning disable 618
            if (_request.isHttpError || _request.isNetworkError)
#pragma warning restore 618
            {
                Debug.Log(_request.responseCode);
                Debug.LogError(_request.error);
            }
            else
            {
                action.Invoke(_request.downloadHandler.text);
            }
        }

        [LabelText("发送下载请求")]
        public void SendDownRequest(string url, Action<byte[]> action)
        {
            StartCoroutine(DownRequest(url, action));
        }

        IEnumerator DownRequest(string url, Action<byte[]> action)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isDone)
            {
                action.Invoke(unityWebRequest.downloadHandler.data);
            }
        }
    }
}