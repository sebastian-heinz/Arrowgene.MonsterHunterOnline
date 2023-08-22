namespace Arrowgene.MonsterHunterOnline.Service.Tdr;

/**
 * Tencent Data Representation - Type-length-value or Tag-Length-Value
 */
public static class Tlv
{
    public static uint MakeTag(int id, TlvType type)
    {
        return (uint)(id << 4 | (int)type);
    }

    public static uint GetFieldId(uint tagId)
    {
        return tagId >> 4;
    }

    public static uint GetTypeId(uint tagId)
    {
        return tagId & 15u;
    }
}