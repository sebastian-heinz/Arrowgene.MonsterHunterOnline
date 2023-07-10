namespace Arrowgene.MonsterHunterOnline.Service.Tdr;

public class TdrTlv
{
    public static uint MakeTag(int id, TdrTlvType type)
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