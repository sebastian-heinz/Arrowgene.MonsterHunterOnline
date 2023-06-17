namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public interface IRemoteDataInfo : IStructure
{
    public RemoteDataType DataType { get; }
}