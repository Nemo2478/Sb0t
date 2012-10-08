﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Cryptography;

namespace core.LinkHub
{
    class HubOutbound
    {
        public static byte[] LinkError(LinkError code)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteByte((byte)code);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_ERROR);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubAck(Leaf leaf)
        {
            byte[] guid = leaf.Guid.ToByteArray();

            using (MD5 md5 = MD5.Create())
                guid = md5.ComputeHash(guid);

            byte[] key = leaf.IV.Concat(leaf.Key).ToArray();

            for (int i = 0; i < guid.Length; i += 2)
                key = Crypto.e67(key, BitConverter.ToUInt16(guid, i));

            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteByte((byte)(LeafPool.Leaves.Count - 1));
            packet.WriteBytes(key);
            packet.WriteUInt32(leaf.Ident);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_ACK);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubLeafDisconnected(Leaf x, Leaf leaf)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(leaf.Ident);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_LEAF_DISCONNECTED);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubLeafConnected(Leaf x, Leaf leaf)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(leaf.Ident);
            packet.WriteString(x, leaf.Name);
            packet.WriteIP(leaf.ExternalIP);
            packet.WriteUInt16(leaf.Port);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_LEAF_CONNECTED);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubUserlistItem(Leaf x, uint ident, LinkUser user)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(ident);
            packet.WriteString(x, user.Name);
            packet.WriteString(x, user.Version);
            packet.WriteGuid(user.Guid);
            packet.WriteUInt16(user.FileCount);
            packet.WriteIP(user.ExternalIP);
            packet.WriteIP(user.LocalIP);
            packet.WriteUInt16(user.Port);
            packet.WriteString(x, user.DNS);
            packet.WriteByte((byte)(user.Browsable ? 1 : 0));
            packet.WriteByte(user.Age);
            packet.WriteByte(user.Sex);
            packet.WriteByte(user.Country);
            packet.WriteString(x, user.Region);
            packet.WriteByte((byte)user.Level);
            packet.WriteUInt16(user.Vroom);
            packet.WriteByte((byte)(user.CustomClient ? 1 : 0));
            packet.WriteByte((byte)(user.Muzzled ? 1 : 0));
            packet.WriteByte((byte)(user.WebClient ? 1 : 0));
            packet.WriteByte((byte)(user.Encrypted ? 1 : 0));
            packet.WriteByte((byte)(user.Registered ? 1 : 0));
            packet.WriteByte((byte)(user.Idle ? 1 : 0));
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_USERLIST_ITEM);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubCustomName(Leaf x, uint ident, LinkUser user)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(ident);
            packet.WriteString(x, user.Name);
            packet.WriteString(x, user.CustomName, false);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_CUSTOM_NAME);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubAvatar(Leaf x, uint ident, LinkUser user)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(ident);
            packet.WriteString(x, user.Name);
            packet.WriteBytes(user.Avatar);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_AVATAR);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubPersonalMessage(Leaf x, uint ident, LinkUser user)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(ident);
            packet.WriteString(x, user.Name);
            packet.WriteString(x, user.PersonalMessage, false);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_PERSONAL_MESSAGE);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubPart(Leaf x, uint ident, LinkUser user)
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            packet.WriteUInt32(ident);
            packet.WriteString(x, user.Name);
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_PART);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }

        public static byte[] HubPong()
        {
            TCPPacketWriter packet = new TCPPacketWriter();
            byte[] buf = packet.ToLinkPacket(LinkMsg.MSG_LINK_HUB_PONG);
            packet = new TCPPacketWriter();
            packet.WriteBytes(buf);
            return packet.ToAresPacket(TCPMsg.MSG_LINK_PROTO);
        }
    }
}