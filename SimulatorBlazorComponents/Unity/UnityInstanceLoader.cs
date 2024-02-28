using CommunityToolkit.Mvvm.Messaging;
using Microsoft.JSInterop;

namespace SimulatorBlazorComponents.Unity
{
    public class UnityInstanceEventArgs : EventArgs
    {
        public bool IsLoaded { get; set; }
    }

    public class UnityInstanceLoader
    {
        private readonly IJSRuntime _js;

        public delegate void OnUnityInstanceLoaded(object sender, UnityInstanceEventArgs e);
        public event OnUnityInstanceLoaded? UnityInstanceLoaded;

        public UnityInstanceLoader(IJSRuntime js)
        {
            _js = js;
        }

        public bool IsInstanceLoaded { get; private set; } = false;

        public async Task<bool> LoadUnityInstance()
        {
            if (IsInstanceLoaded == true)
            {
                return true;
            }

            var wasSuccess = await _js.InvokeAsync<bool>("setupUnityInstance");

            IsInstanceLoaded = wasSuccess;

            var eventArgs = new UnityInstanceEventArgs() { IsLoaded = wasSuccess };
            UnityInstanceLoaded?.Invoke(this, eventArgs);
            WeakReferenceMessenger.Default.Send(eventArgs);
            return wasSuccess;
        }
    }
}