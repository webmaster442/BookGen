using BookGen.Vfs;

namespace Bookgen.Lib.JsInterop;

public sealed class QrCodeInterop : JavascriptInterop
{
    public QrCodeInterop(IAssetSource assetSource)
    {
        string qrcodeJs = assetSource.GetAsset(BundledAssets.QrCodeJs);
        Execute(qrcodeJs);
    }

    public string GenerateQrCode(string message, string foreground = "#000")
    {
        string cmd = $"new QRCode({{content: \"{message}\", padding: 2, color: \"{foreground}\"}}).svg();";
        return ExecuteAndGetResult(cmd);
    }
}