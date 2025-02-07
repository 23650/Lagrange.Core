using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(RequestFriendSearchEvent))]
[Service("OidbSvcTrpcTcp.0x972_6")]
internal class RequestFriendSearchService : BaseService<RequestFriendSearchEvent>
{
    protected override bool Build(RequestFriendSearchEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x972_6>(new OidbSvcTrpcTcp0x972_6
        {
            TargetUin = input.TargetUin.ToString(),
            Settings = new OidbSvcTrpcTcp0x972_6Settings
            {
                Field4 = 25,
                Field11 = "",
                Setting = "{\"search_by_uid\":true, \"scenario\":\"related_people_and_groups_panel\"}"
            }
        });
        
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);
        
        extraPackets = null;
        return true;
    }
    
    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, 
        out RequestFriendSearchEvent output, out List<ProtocolEvent>? extraEvents)
    {
        output = RequestFriendSearchEvent.Result(0);
        extraEvents = null;
        return true;
    }
}