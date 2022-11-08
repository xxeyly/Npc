using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace XFramework
{
    public class HttpFrameComponent : FrameComponent
    {
        private static HttpFrameComponent Instance;
        private UnityWebRequest _request;

        /// <summary>
        /// Http请求模式
        /// </summary>
        public enum HttpRequestMethod
        {
            Get,
            Put,
            Delete,
            Post
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
        public void SendHttpUnityWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action, string requestData = "")
        {
            StartCoroutine(UnityHttpWebRequest(url, requestMethod, action, requestData));
        }


        IEnumerator UnityHttpWebRequest(string url, HttpRequestMethod requestMethod, Action<string> action, string requestData = "")
        {
            if (requestData.Length == 0)
            {
                requestData += requestMethod;
            }

            byte[] dataByte = Encoding.UTF8.GetBytes(requestData);
            _request = new UnityWebRequest(url, requestMethod.ToString());
            _request.uploadHandler = new UploadHandlerRaw(dataByte);
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
    }
}