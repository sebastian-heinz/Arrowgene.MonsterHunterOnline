using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AttrBaseData : Structure, ICsStructure
{
    private CS_PROP_SYNC_TYPE _type;

    private int _int;
    private float _float;
    private string _string;
    private bool _bool;
    private CSVec3 _vec3;
    private ulong _uInt64;

    public AttrBaseData()
    {
        _int = 0;
        _float = 0;
        _string = "";
        _bool = false;
        _vec3 = new CSVec3();
        _uInt64 = 0;
    }
    
    public int Int
    {
        get => _int;
        set
        {
            _int = value;
            _type = CS_PROP_SYNC_TYPE.CS_PROP_SYNC_INT;
        }
    }

    public float Float
    {
        get => _float;
        set
        {
            _float = value;
            _type = CS_PROP_SYNC_TYPE.CS_PROP_SYNC_FLOAT;
        }
    }

    public string String
    {
        get => _string;
        set
        {
            _string = value;
            _type = CS_PROP_SYNC_TYPE.CS_PROP_SYNC_STRING;
        }
    }

    public bool Bool
    {
        get => _bool;
        set
        {
            _bool = value;
            _type = CS_PROP_SYNC_TYPE.CS_PROP_SYNC_BOOL;
        }
    }

    public CSVec3 Vec3
    {
        get => _vec3;
        set
        {
            _vec3 = value;
            _type = CS_PROP_SYNC_TYPE.CS_PROP_SYNC_VEC3;
        }
    }

    public ulong UInt64
    {
        get => _uInt64;
        set
        {
            _uInt64 = value;
            _type = CS_PROP_SYNC_TYPE.CS_PROP_SYNC_UINT64;
        }
    }

    public  void WriteCs(IBuffer buffer)
    {
        WriteUInt16(buffer, (ushort)_type);
        switch (_type)
        {
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_INT:
                WriteInt32(buffer, _int);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_FLOAT:
                WriteFloat(buffer, _float);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_STRING:
                WriteString(buffer, _string);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_BOOL:
                WriteByte(buffer, _bool ? (byte)1 : (byte)0);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_VEC3:
                WriteCsStructure(buffer, _vec3);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_UINT64:
                WriteUInt64(buffer, _uInt64);
                break;
        }
    }

    public void ReadCs(IBuffer buffer)
    {
        _type = (CS_PROP_SYNC_TYPE)ReadUInt16(buffer);
        switch (_type)
        {
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_INT:
                _int = ReadInt32(buffer);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_FLOAT:
                _float = ReadFloat(buffer);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_STRING:
                _string = ReadString(buffer);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_BOOL:
                _bool = ReadByte(buffer) != 0;
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_VEC3:
                _vec3 = ReadCsStructure(buffer, _vec3);
                break;
            case CS_PROP_SYNC_TYPE.CS_PROP_SYNC_UINT64:
                _uInt64 = ReadUInt64(buffer);
                break;
        }
    }
}