using System;

namespace EAN
{
    internal static class MatrixHelper
    {
        public static void Display<T>(Matrix<T> matrix)

        {
            if (matrix == null)
                throw new ArgumentNullException(nameof(matrix));

            for (int i = 0; i < matrix.RowCount; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < matrix.ColumnCount; j++)
                    Console.Write($"{matrix[i, j]:F20} ");
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static Matrix<T> GenerateRandom<T>(int rowCount, int columnCount, T minValue, T maxValue)

        {
            if (rowCount < 1)
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            if (columnCount < 1)
                throw new ArgumentOutOfRangeException(nameof(columnCount));

            dynamic min = minValue;
            dynamic max = maxValue;

            if (min > max)
                throw new InvalidOperationException("Min value can not be grater than max value.");

            var matrix = new T[rowCount, columnCount];
            var random = new Random();

            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                {
                    if (typeof(T) == typeof(int))
                        matrix[i, j] = random.Next(min, max);
                    else
                        matrix[i, j] = (T)(dynamic)random.NextDouble() * (max - min) + min;
                }

            return new Matrix<T>(matrix);
        }

        public static Matrix<T> ReadFromConsole<T>(int rowCount, int columnCount)

        {
            if (rowCount < 1)
                throw new ArgumentOutOfRangeException(nameof(rowCount));
            if (columnCount < 1)
                throw new ArgumentOutOfRangeException(nameof(columnCount));

            var matrix = new T[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Console.Write($"Podaj wartość na pozycji [{i + 1},{j + 1}]: ");
                    matrix[i, j] = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                }
            }

            return new Matrix<T>(matrix);
        }
    }
}