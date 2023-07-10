namespace Arrowgene.MonsterHunterOnline.Service.Tdr;

public enum TdrTlvType : byte
{
    ID_VARINT = 0,
    ID_1_BYTE,
    ID_2_BYTE,
    ID_4_BYTE,
    ID_8_BYTE,
    ID_LENGTH_DELIMITED
}