using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service;
using Xunit;

namespace Arrowgene.MonsterHunterOnline.Test.Service;

public class UtilTest
{
    [Fact]
    public void TestChunk()
    {
        List<int> ints = new List<int>();
        for (int i = 0; i < 55; i++)
        {
            ints.Add(i);
        }

        List<List<int>> chunks = Util.Chunk(ints, 25);
        for (int i = 0; i < chunks.Count; i++)
        {
            if (i == chunks.Count - 1)
            {
                Assert.Equal(5, chunks[i].Count);
                continue;
            }

            Assert.Equal(25, chunks[i].Count);
        }
    }
}