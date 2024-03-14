namespace Authentication.Service;

public static class HttpContextExtensions
{
    public static (string RemoteAddress, string LocalAddress) GetIpAddresses(this HttpContext context)
    {
        var ipRemoteHost = context.Connection.RemoteIpAddress?.ToString();
        var portRemote = context.Connection.RemotePort.ToString();
        var ipRemoteAddress = ipRemoteHost + ":" + portRemote;

        var ipLocalHost = context.Connection.LocalIpAddress?.ToString();
        var portLocal = context.Connection.LocalPort.ToString();
        var ipLocalAddress = ipLocalHost + ":" + portLocal;

        return (ipRemoteAddress, ipLocalAddress);
    }
}
