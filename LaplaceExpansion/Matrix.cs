using System;

namespace EAN
{
    internal class Matrix<T>

    {
        private T[,] matrix;

        public Matrix(T[,] matrix)
        {
            this.RowCount = matrix.GetLength(0);
            this.ColumnCount = matrix.GetLength(1);

            this.matrix = new T[this.RowCount, this.ColumnCount];

            for (int i = 0; i < this.RowCount; i++)
                for (int j = 0; j < this.ColumnCount; j++)
                    this.matrix[i, j] = matrix[i, j];
        }

        public int ColumnCount { get; private set; }

        public int RowCount { get; private set; }

        public T this[int row, int column]
        {
            get { return this.matrix[row, column]; }
        }

        public T ComputeDeterminant()
        {
            if (RowCount != ColumnCount)
                throw new InvalidOperationException("Determinant exists only for square matrices.");

            if (RowCount == 1)
                return (T)(dynamic)this[0, 0];

            dynamic result = default(T);
            for (int i = 0; i < ColumnCount; i++)
                result += (dynamic)this[0, i] * (i % 2 == 0 ? 1 : -1) * GetMirror(this, 0, i).ComputeDeterminant();
            return result;
        }

        private static Matrix<E> GetMirror<E>(Matrix<E> matrix, int rowIndex, int columnIndex)
        {
            E[,] newMatrix = new E[matrix.RowCount - 1, matrix.ColumnCount - 1];

            for (int i1 = 0, i2 = 0; i1 < matrix.RowCount; i1++)
            {
                if (i1 == rowIndex)
                    continue;

                for (int j1 = 0, j2 = 0; j1 < matrix.ColumnCount; j1++)
                {
                    if (j1 == columnIndex)
                        continue;

                    newMatrix[i2, j2] = matrix[i1, j1];

                    j2++;
                }

                i2++;
            }

            return new Matrix<E>(newMatrix);
        }
    }
}