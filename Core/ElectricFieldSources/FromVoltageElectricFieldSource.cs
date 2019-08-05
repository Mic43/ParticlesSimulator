using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Core.Utils;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;

namespace Core.ElectricFieldSources
{
    public class FromVoltageElectricFieldSource : IElectricFieldSource<float> 
    {
        private readonly byte[,] voltageMap;
        private readonly float minVoltage;
        private readonly float maxVoltage;
        private readonly Vector<float> origin;
        private readonly float unitSize;

        private readonly Lazy<(short x, short y)[,]> _intensityMap;


        public FromVoltageElectricFieldSource(byte[,] voltageMap, float minVoltage, float maxVoltage, float unitSize) 
            : this(voltageMap, minVoltage, maxVoltage, unitSize, Vector2D.Zero)
        {
        }

        public FromVoltageElectricFieldSource(byte[,] voltageMap, float minVoltage, float maxVoltage, float unitSize,
            Vector<float> origin)
        {
            this.voltageMap = voltageMap;
            this.minVoltage = minVoltage;
            this.maxVoltage = maxVoltage;
            this.origin = origin;
            this.unitSize = unitSize;
            _intensityMap = new Lazy<(short x, short y)[,]>(InitIntensityMap);
        }

        private (short x, short y)[,] InitIntensityMap()
        {

            var matrix = new Matrix<byte>(voltageMap);

            int sizeX = voltageMap.GetLength(0);
            int sizeY = voltageMap.GetLength(1);

            var outX = new Matrix<int>(sizeX, sizeY);
            var outY = new Matrix<int>(sizeX, sizeY);


            CvInvoke.Sobel(matrix, outX, DepthType.Cv16S, 1, 0);
            CvInvoke.Sobel(matrix, outY, DepthType.Cv16S, 0, 1);

            (short x, short y)[,] intensityMap = new (short x, short y)[sizeX,sizeY];

            //TODO: scalling with min and maxVoltage??
            var arrayX = outX.Mat.GetData();
            var arrayY = outY.Mat.GetData();

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    intensityMap[i,j] = (
                        (short) arrayX.GetValue(i, j), 
                        (short)arrayY.GetValue(i, j)
                        );
                }
            }

            return intensityMap;
        }

        public Vector<float> GetIntensity(Vector<float> location)
        {
            Vector<float> vector = (location - origin) * (1/unitSize);
            (int x, int y) position = ((int) vector.X(), (int) vector.Y());

            if (position.x < 0 || position.x >= _intensityMap.Value.GetLength(0)
                               || position.y < 0 || position.y >= _intensityMap.Value.GetLength(1))
                return Vector2D.Zero;

            var intensity = _intensityMap.Value[position.x, position.y];
            var maxVoltageMapValue = 256;
            return Vector2D.Create(intensity.x, intensity.y) * (maxVoltage - minVoltage) *(1.0f / maxVoltageMapValue);
        }
    }
}