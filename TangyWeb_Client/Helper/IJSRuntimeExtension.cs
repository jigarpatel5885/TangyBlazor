using Microsoft.JSInterop;

namespace TangyWeb_Client.Helper
{
    public static class IJSRuntimeExtension
    {
        public static async ValueTask ToasterSuccess(this IJSRuntime jsRuntime,string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "success", message);
        }
        public static async ValueTask ToasterFailure(this IJSRuntime jsRuntime,string message)
        {
            await jsRuntime.InvokeVoidAsync("ShowToastr", "error", message);
        }
    }
}
