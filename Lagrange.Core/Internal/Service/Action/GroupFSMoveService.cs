using Lagrange.Core.Common;
using Lagrange.Core.Internal.Event.Protocol;
using Lagrange.Core.Internal.Event.Protocol.Action;
using Lagrange.Core.Internal.Packets.Service.Oidb;
using Lagrange.Core.Internal.Packets.Service.Oidb.Request;
using Lagrange.Core.Utility.Binary;
using ProtoBuf;

namespace Lagrange.Core.Internal.Service.Action;

[EventSubscribe(typeof(GroupFSMoveEvent))]
[Service("OidbSvcTrpcTcp.0x6d6_5")]
internal class GroupFSMoveService : BaseService<GroupFSMoveEvent>
{
    protected override bool Build(GroupFSMoveEvent input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device,
        out BinaryPacket output, out List<BinaryPacket>? extraPackets)
    {
        var packet = new OidbSvcTrpcTcpBase<OidbSvcTrpcTcp0x6D6_5>(new OidbSvcTrpcTcp0x6D6_5
        {
            Move = new OidbSvcTrpcTcp0x6D6_5Move
            {
                GroupUin = input.GroupUin,
                AppId = 7,
                BusId = 102,
                fileId = input.FileId,
                ParentDirectory = input.ParentDirectory,
                TargetDirectory = input.TargetDirectory
            }
        }, false, true);

        var stream = new MemoryStream();
        Serializer.Serialize(stream, packet);
        output = new BinaryPacket(stream);

        extraPackets = null;
        return true;
    }

    protected override bool Parse(byte[] input, BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device, out GroupFSMoveEvent output,
        out List<ProtocolEvent>? extraEvents)
    {
        var packet = Serializer.Deserialize<OidbSvcTrpcTcpResponse<byte[]>>(input.AsSpan());
        
        output = GroupFSMoveEvent.Result((int)packet.ErrorCode);
        extraEvents = null;
        return true;
    }
}