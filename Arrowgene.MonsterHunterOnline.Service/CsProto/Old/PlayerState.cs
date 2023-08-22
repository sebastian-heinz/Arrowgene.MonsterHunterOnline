using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;
using Arrowgene.MonsterHunterOnline.Service.Tdr;

namespace Arrowgene.MonsterHunterOnline.Service;

/// <summary>
///  this is a temporary holder for central management of packet and information,
/// it can be considered the "playground" for now.
///
/// On*- function are lifecycle hooks
/// Send*- functions are to send specific data that has been consistently populated
/// </summary>
public class PlayerState
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(PlayerState));

    private Client _client;
    public static Server Server;
    public int levelId { get; set; }
    public int prevLevelId { get; set; }
    public CSVec3 Position { get; set; }

    public CSVec3 InitSpawnPos = new CSVec3()
//  {
//      x = 1588.4813f,
//      y = 1593.0623f,
//      z = 142.93517f
//  };
    {
        x = 404.91379f,
        y = 396.74976f,
        z = 85.0f
    };

    public int InitLevelId = 150101;


    public PlayerState(Client client)
    {
        _client = client;


        StreamBuffer ast = new StreamBuffer();
        ast.WriteByte((byte)TlvMagic.NoVariant);

        int sizePos = ast.Position;
        ast.WriteInt32(0, Endianness.Big); // size

        // case 0
        uint tag = Tlv.MakeTag(2, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(5, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(1, Endianness.Big);
        }

        // case 1
        // skips X bytes
        tag = Tlv.MakeTag(3, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(11, Endianness.Big);

        // case 2
        // read int32
        tag = Tlv.MakeTag(4, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(12, Endianness.Big);


        // case 3 / skip bytes??
        // read int32
        tag = Tlv.MakeTag(5, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(13, Endianness.Big);


        // case 4
        // read int32
        tag = Tlv.MakeTag(6, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(14, Endianness.Big);


        // case 5
        // read int32
        tag = Tlv.MakeTag(7, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(15, Endianness.Big);


        // case 6
        // read int32
        tag = Tlv.MakeTag(8, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(16, Endianness.Big);


        // case 7
        // read int32
        tag = Tlv.MakeTag(9, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(17, Endianness.Big);


        // case 8
        // read int32
        tag = Tlv.MakeTag(10, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(18, Endianness.Big);


        // case 9
        // read int32
        tag = Tlv.MakeTag(11, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(19, Endianness.Big);

        // case 10
        // read int32
        tag = Tlv.MakeTag(12, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(20, Endianness.Big);

        // case 11
        // read int32
        tag = Tlv.MakeTag(13, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(21, Endianness.Big);

        // case 12
        // read int32
        tag = Tlv.MakeTag(14, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(22, Endianness.Big);

        // case 13
        // read int32
        tag = Tlv.MakeTag(15, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(23, Endianness.Big);

        // case 14
        // read int32
        tag = Tlv.MakeTag(16, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(24, Endianness.Big);

        // case 15
        // read int32
        tag = Tlv.MakeTag(17, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(25, Endianness.Big);
        }

        // case 16 - 0x10
        // read int32
        tag = Tlv.MakeTag(18, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(26, Endianness.Big);
        }

        // 17 - readx7 1878525
        tag = Tlv.MakeTag(19, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(27, Endianness.Big);
        }

        //18 - readint32 1878660
        tag = Tlv.MakeTag(20, TlvType.ID_2_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt16(28, Endianness.Big);

//19 - readint32 1878706
        tag = Tlv.MakeTag(21, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(29, Endianness.Big);

// 20 - readx7 1878752
        tag = Tlv.MakeTag(22, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(30, Endianness.Big);
        }

// 21 - readx7 1878893
        tag = Tlv.MakeTag(23, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(31, Endianness.Big);
        }

        //22 - readx7 1879037
        tag = Tlv.MakeTag(24, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(32, Endianness.Big);
        }

        //23 - readx7 1879181
        tag = Tlv.MakeTag(25, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(33, Endianness.Big);
        }

//24 - skip 1900773
        tag = Tlv.MakeTag(26, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(34, Endianness.Big);


        tag = Tlv.MakeTag(27, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(34, Endianness.Big);


        tag = Tlv.MakeTag(28, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(34, Endianness.Big);

        tag = Tlv.MakeTag(29, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(34, Endianness.Big);


        tag = Tlv.MakeTag(30, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(34, Endianness.Big);

// 29
        tag = Tlv.MakeTag(31, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(34, Endianness.Big);


        // 30 - readx7 1879325

        tag = Tlv.MakeTag(32, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(10, Endianness.Big);
        }

        tag = Tlv.MakeTag(33, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(10, Endianness.Big);
        }

        tag = Tlv.MakeTag(34, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(10, Endianness.Big);
        }

        tag = Tlv.MakeTag(35, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
        for (int i = 0; i < 7; i++)
        {
            ast.WriteInt32(10, Endianness.Big);
        }

        //34 - readint32 1879901
        tag = Tlv.MakeTag(36, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(35, Endianness.Big);


        for (int i = 37; i < 200; i++)
        {
            tag = Tlv.MakeTag(i, TlvType.ID_4_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt32(7 * 4, Endianness.Big); // size, max 7*4
            for (int j = 0; j < 7; j++)
            {
                ast.WriteInt32(2, Endianness.Big);
            }
        }


        int size = ast.Position - sizePos;
        ast.Position = sizePos;
        ast.WriteInt32(size, Endianness.Big); // size
    }

    
    public static byte[] GetSkill()
    {
        StreamBuffer ast = new StreamBuffer();
        ast.WriteByte((byte)TlvMagic.NoVariant);

        int sizePos = ast.Position;
        ast.WriteInt32(0, Endianness.Big); // size

        // case 0
        uint tag = Tlv.MakeTag(1 /* id(1) -1 = 0 */ , TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt32(0xAACCDD, Endianness.Big);
        
        // case 1
        tag = Tlv.MakeTag(2 /* id(2) -1 = 1 */ , TlvType.ID_2_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt16(0xBB, Endianness.Big);
        
         // case 2
        tag = Tlv.MakeTag(3, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        int subStartPos = ast.Position;
        ast.WriteInt32(0, Endianness.Big); // sub length

        for (int subEntryIndex = 0; subEntryIndex < 5; subEntryIndex++)
        {
            int subEntryStartPos = ast.Position;
            ast.WriteInt32(0, Endianness.Big); // sub entry length

            // sub structure - max 3
            //case s0
            tag = Tlv.MakeTag(1, TlvType.ID_4_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt32(0, Endianness.Big);
            //case s1
            tag = Tlv.MakeTag(2, TlvType.ID_4_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt32(0, Endianness.Big);
            //case s2
            tag = Tlv.MakeTag(3, TlvType.ID_4_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt32(0, Endianness.Big);

           
            int subEntryEndPos = ast.Position;
            int subEntrySize = ast.Position - subEntryStartPos - 4;
            ast.Position = subEntryStartPos;
            ast.WriteInt32(subEntrySize, Endianness.Big); // size
            ast.Position = subEntryEndPos;
            // end sub entry
        }


        int subEndPos = ast.Position;
        int subSize = ast.Position - subStartPos - 4;
        ast.Position = subStartPos;
        ast.WriteInt32(subSize, Endianness.Big); // size
        ast.Position = subEndPos;
        // end sub structure
        
        
        int size = ast.Position - sizePos + 1;
        ast.Position = sizePos;
        ast.WriteInt32(size, Endianness.Big); // size
        return ast.GetAllBytes();
        
        
    }
   
    public static byte[] GetEquip()
    {
        StreamBuffer ast = new StreamBuffer();
        ast.WriteByte((byte)TlvMagic.NoVariant);

        int sizePos = ast.Position;
        ast.WriteInt32(0, Endianness.Big); // size

        // case 1
        uint tag = Tlv.MakeTag(1, TlvType.ID_2_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt16(1);

        // case 2
        tag = Tlv.MakeTag(2, TlvType.ID_4_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        int subStartPos = ast.Position;
        ast.WriteInt32(0, Endianness.Big); // sub length

        for (int subEntryIndex = 0; subEntryIndex < 40; subEntryIndex++)
        {
            int subEntryStartPos = ast.Position;
            ast.WriteInt32(0, Endianness.Big); // sub entry length

            // sub structure - max 9
            //case s0
            tag = Tlv.MakeTag(2, TlvType.ID_8_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt64(64439 + subEntryIndex, Endianness.Big);
            //case s1
            tag = Tlv.MakeTag(3, TlvType.ID_4_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt32(64439 + subEntryIndex, Endianness.Big);
            //case s2
            tag = Tlv.MakeTag(4, TlvType.ID_1_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteByte(1);
            //case s3
            tag = Tlv.MakeTag(5, TlvType.ID_2_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt16(1, Endianness.Big);
            //case s4
            tag = Tlv.MakeTag(6, TlvType.ID_2_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteInt16(1, Endianness.Big);
            //case s5
            tag = Tlv.MakeTag(7, TlvType.ID_1_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteByte(1);
            //case s6
            tag = Tlv.MakeTag(8, TlvType.ID_1_BYTE);
            TdrBuffer.WriteVarUInt32(ast, tag);
            ast.WriteByte(1);

            //cases7 -> skip bytes

            //case s8
    //    tag = TdrTlv.MakeTag(10, TdrTlvType.ID_4_BYTE);
    //    TdrBuffer.WriteVarUInt32(ast, tag);
    //    ast.WriteInt32(0x20 * 1, Endianness.Big); // size, max 0x20*1
    //    for (int i = 0; i < 0x20; i++)
    //    {
    //        ast.WriteByte((byte)i);
    //    }

    //    //case s9
    //    tag = TdrTlv.MakeTag(11, TdrTlvType.ID_4_BYTE);
    //    TdrBuffer.WriteVarUInt32(ast, tag);
    //    ast.WriteInt32(0x20 * 4, Endianness.Big); // size, max 0x20*4
    //    for (int i = 0; i < 0x20; i++)
    //    {
    //        ast.WriteInt32(i, Endianness.Big);
    //    }

            int subEntryEndPos = ast.Position;
            int subEntrySize = ast.Position - subEntryStartPos - 4;
            ast.Position = subEntryStartPos;
            ast.WriteInt32(subEntrySize, Endianness.Big); // size
            ast.Position = subEntryEndPos;
            // end sub entry
        }


        int subEndPos = ast.Position;
        int subSize = ast.Position - subStartPos - 4;
        ast.Position = subStartPos;
        ast.WriteInt32(subSize, Endianness.Big); // size
        ast.Position = subEndPos;
        // end sub structure

        // case 3
        tag = Tlv.MakeTag(3, TlvType.ID_2_BYTE);
        TdrBuffer.WriteVarUInt32(ast, tag);
        ast.WriteInt16(0, Endianness.Big);


        int size = ast.Position - sizePos + 1;
        ast.Position = sizePos;
        ast.WriteInt32(size, Endianness.Big); // size
        return ast.GetAllBytes();
    }

    public void SendBruteForceT()
    {
        Thread bruteForce = new Thread(SendBruteForce);
        bruteForce.Start();
    }

    public void SendBruteForce()
    {
        Thread.Sleep(3000);

        List<string> exclude = new List<string>()
        {
            "CatTreatureErr",
            "CatTreatureOpen",
            "XHunterResultNtf",
            "CommanderChgNtf",
            "BattlePunishNtf",
            "ExcellentPointNtf",
            "SensitiveVerify",
            "SensitiveVerifyResult",
            "CatCuisineUnlockNtf",
            "C2STalkExec",
            "C2STalkEnd",
            "S2CTalkErr",
            "S2CTalkExec",
            "EquipPlanUnlockNtf",
            "EquipPlanEditNtf",
            "LineUpStateNtf",
            "GuildMatchSignUpReadyNtf",
            "GuildMatchSignUpListNtf",
            "GuildMatchSignUpAdd",
            "GuildMatchSignUpDel",
            "ScheduleError",
            "ObtainTargetListRes",
            "GuildMatchQualifierFirstNtf",
            "GuildMatchStateNtf",
            "GuildMatchPairListNtf",
            "PkgEncryptData",
            "Ping",
            "HeartBeat",
            "GameManagerCmd",
            "GlobalErrNtf",
            "PkgTimerRecord",
            "PkgTransAntiData",
            "PingReply",
            "PkgChatEncryptData",
            "ZipPkg",
            "DelBuff",
            "BuffParamChange",
            "NotifyInfo",
            "DropClientNotifyInfo",
            "PropSync",
            "TimeOfDayNtf",
            "AttrSync",
            "AttrInfo",
            "FarmSetEquipAvatarNtf",
            "FarmWoodIndexIDMappingNtf",
            "CatTreatureInfo",
            "C2SSpeakWord",
            "C2SSpeakSetSelfDef",
            "C2SSpeakSetAuto",
            "S2CSculptureLibSnapshot",
            "S2CGetSculptureLibs",
            "S2CSculptureErr",
            "S2CSpeakErr",
            "SurrenderVoteNtf",
            "C2SGetSculptureLibs",
            "S2CGetSculpture",
            "S2CSculptureAvatarSnapshot",
            "S2CScriptAddSculpture",
            "S2CSpoorFetchPrize",
            "S2CSpoorErr",
            "S2CSpoorAddPoints",
            "ItemMgrMoveSwapItemsNtf",
        };

        Type type = typeof(NewCsPacket);
        List<MethodInfo> collectedMethods = new List<MethodInfo>();
        foreach (MethodInfo mi in type.GetMethods())
        {
            if (exclude.Contains(mi.Name))
            {
                continue;
            }

            if (mi.Name == "Equals"
                || mi.Name == "GetType"
                || mi.Name == "ToString"
                || mi.Name == "GetHashCode")
            {
                continue;
            }

            if (mi.Name.EndsWith("Req"))
            {
                continue;
            }

            if (mi.Name.StartsWith("C2S"))
            {
                continue;
            }

            if (mi.Name.EndsWith("Rsp"))
            {
                continue;
            }

            collectedMethods.Add(mi);
        }

        int start = 0;
        int current = 0;
        int total = collectedMethods.Count;
        foreach (MethodInfo mi in collectedMethods)
        {
            if (current < start)
            {
                current++;
                continue;
            }

            ParameterInfo[] parameters = mi.GetParameters();
            if (parameters.Length != 1)
            {
                Logger.Error($"parameters.Length != 1 ({mi})");
                continue;
            }

            object parameterInstance = CreateInstance(parameters[0].ParameterType);
            object ret = mi.Invoke(null, new object[] { parameterInstance });
            if (ret is CsPacket csPacket)
            {
                Logger.Error($"Sending {mi.Name} ({current}/{total})");
                try
                {
                    _client.SendCsPacket(csPacket);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }

                Thread.Sleep(1000);
                current++;
            }
            else
            {
                Logger.Error($"ret is NOT CsPacket csPacket({mi})");
            }
        }
    }

    private object CreateInstance(Type type)
    {
        if (type.IsInterface)
        {
            Type concrete = null;
            foreach (Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
            {
                if (!t.IsInterface && !t.IsAbstract && type.IsAssignableFrom(t))
                {
                    concrete = t;
                    break;
                }
            }

            if (concrete == null)
            {
                Logger.Error($"Interface ({type}) can not be created, no implementation found");
                return null;
            }

            Logger.Info($"Interface ({type}) can not be created, using implementation {concrete}");
            type = concrete;
        }

        ConstructorInfo[] cis = type.GetConstructors();

        if (cis.Length <= 0)
        {
            return Activator.CreateInstance(type);
        }

        ParameterInfo[] pis = cis[0].GetParameters();
        if (pis.Length <= 0)
        {
            return Activator.CreateInstance(type);
        }

        object[] paramInstances = new object[pis.Length];
        for (int i = 0; i < pis.Length; i++)
        {
            paramInstances[i] = CreateInstance(pis[i].ParameterType);
        }

        return Activator.CreateInstance(type, paramInstances);
    }
}