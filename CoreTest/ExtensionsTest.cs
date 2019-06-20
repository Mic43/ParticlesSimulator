using System.Linq;
using System.Numerics;
using Core;
using Core.ElectricFieldSources;
using Core.Utils;
using Xunit;

namespace CoreTest
{
    public class ExtensionsTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(11)]

        public void CreatePositionsDistributionCountTest(int count)
        {
            var positionsDistribution = Extensions.CreatePositionsDistribution(
                Vector<float>.Zero, Vector<float>.One, count);
            Assert.Equal(positionsDistribution.ToList().Count, count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(11)]

        public void ShouldHaveEqualDistanceBetweenPositions(int count)
        {
            var start = Vector2D.Zero;
            var end = Vector2D.One;
            var positionsDistribution = Extensions.CreatePositionsDistribution(start, end, count)
                .ToList();

            positionsDistribution.Insert(0,start);
            positionsDistribution.Add(end);

            // var first = start;
            for (int i = 0; i < count; i++)
            {
                var first = positionsDistribution[i];
                var second = positionsDistribution[i + 1];
                var last = positionsDistribution[i + 2];

                Assert.Equal((second - first).Length(),
                    (last - second).Length(),2);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(11)]
       
        public void PointsShouldBetweenStartAndEnd(int count)
        {
            var start = Vector2D.Zero;
            var end = Vector2D.One;
            var positionsDistribution = Extensions.CreatePositionsDistribution(start, end, count)
                .ToList();


            Assert.True(positionsDistribution.All(x => Vector.GreaterThanOrEqualAll(x, start)
                                                       && Vector.LessThanOrEqualAll(x, end)));


        }
    }
}