
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace SimulatorBlazorComponents.Unity
{
    public class UnityBridge
    {
        const string JSFuncName = "SendMessageToUnity";

        private readonly IJSRuntime _js;

        public UnityBridge(IJSRuntime js)
        {
            _js = js;
        }

        public async Task SendMessage(string gameObject, string unityMethodName, params object[] args)
        {
            await _js.InvokeVoidAsync(JSFuncName, gameObject, unityMethodName, args);
        }

        public async ValueTask<T> SendMessage<T>(string gameObject, string unityMethodName, params object[] args)
        {
            return await _js.InvokeAsync<T>(JSFuncName, gameObject, unityMethodName, args);
        }

        public Task Pipe<T>(string gameObject, string unityMethodName, T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return SendMessage(gameObject, unityMethodName, json);
        }

        public async Task CallOnInstance(string functionName, params object[] args)
        {
            await _js.InvokeVoidAsync("callOnInstance", functionName, args);
        }
    }
}