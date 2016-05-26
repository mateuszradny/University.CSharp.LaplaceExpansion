using EAN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Console;

namespace EANMatrix
{
    internal static class Program
    {
        private static Matrix<T> GenerateRandomMatrix<T>()
        {
            WriteLine("Podaj stopień macierzy: ");
            Write("-> ");
            var x = int.Parse(ReadLine().Trim());

            WriteLine("Podaj minimalną wartość elementu: ");
            Write("-> ");
            var min = (T)Convert.ChangeType(ReadLine().Trim(), typeof(T));

            WriteLine("Podaj maksymalną wartość elementu: ");
            Write("-> ");
            var max = (T)Convert.ChangeType(ReadLine().Trim(), typeof(T));

            var matrix = MatrixHelper.GenerateRandom<T>(x, x, min, max);

            WriteLine("Wyświetlić macierz: ");
            WriteLine("\t[1] Tak");
            WriteLine("\t[2] Nie");
            Write("-> ");
            x = int.Parse(ReadLine().Trim());

            switch (x)
            {
            case 1:
                MatrixHelper.Display(matrix);
                break;

            case 2:
                break;

            default:
                throw new Exception("Invalid inputs.");
            }

            return matrix;
        }

        private static void Main()
        {
            var r = new Matrix<double>(new double[,] {
                { 1.0, 1.5, 2.0 },
                { 0.9, 0.7, 0.5 },
                { 9.9, 1.7, 1.1 },
            }).ComputeDeterminant();

            Console.WriteLine(DoubleConverter.ToExactString(r));

            start:
            try
            {
                WriteLine("Obliczanie wyznacznika metodą Laplace'a by Mateusz Radny");
                WriteLine();

                var type = ReadType();
                var matrix = typeof(Program).GetMethod(nameof(ReadMatrix), BindingFlags.Static | BindingFlags.NonPublic)
                                            .MakeGenericMethod(type)
                                            .Invoke(null, new object[] { });

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var result = ((dynamic)matrix).ComputeDeterminant();
                stopwatch.Stop();

                WriteLine("Czas obliczeń: {0}, wynik: {1}", stopwatch.Elapsed, result);
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message);
            }

            ReadLine();
            goto start;
        }

        private static Matrix<T> ReadMatrix<T>()

        {
            WriteLine("Skąd chcesz wczytać macierz: ");
            WriteLine("\t[1] Konsola");
            WriteLine("\t[2] Plik");
            WriteLine("\t[3] Macierz losowa");
            Write("-> ");

            var choice = int.Parse(ReadLine().Trim());

            switch (choice)
            {
            case 1: return ReadMatrixFromConsole<T>();
            case 2: return ReadMatrixFromFile<T>();
            case 3: return GenerateRandomMatrix<T>();
            }

            throw new Exception("Invalid inputs.");
        }

        private static Matrix<T> ReadMatrixFromConsole<T>()
        {
            WriteLine("Podaj stopień macierzy: ");
            Write("-> ");
            var x = int.Parse(ReadLine().Trim());

            var matrix = MatrixHelper.ReadFromConsole<T>(x, x);

            WriteLine("Wyświetlić macierz: ");
            WriteLine("\t[1] Tak");
            WriteLine("\t[2] Nie");
            Write("-> ");
            x = int.Parse(ReadLine().Trim());

            switch (x)
            {
            case 1:
                MatrixHelper.Display(matrix);
                break;

            case 2:
                break;

            default:
                throw new Exception("Invalid inputs.");
            }

            return matrix;
        }

        private static Matrix<T> ReadMatrixFromFile<T>()
        {
            WriteLine("Podaj ścieżkę do pliku: ");
            Write("-> ");
            var path = ReadLine();

            var matrixRows = File.ReadAllLines(path);
            var elements = matrixRows.Select(x => x.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(y => (T)Convert.ChangeType(y.Trim(), typeof(T))).ToArray()).ToArray();

            if (matrixRows.Length != elements[0].Length)
                throw new Exception("Determinant exists only for square matrices.");

            var matrix = new T[elements.Length, elements.Length];
            for (int i = 0; i < elements.Length; i++)
                for (int j = 0; j < elements.Length; j++)
                    matrix[i, j] = elements[i][j];

            return new Matrix<T>(matrix);
        }

        private static Type ReadType()
        {
            WriteLine("Jaki typ danych chcesz użyć: ");
            WriteLine("\t[1] int");
            WriteLine("\t[2] float - 7 digits (32 bit)");
            WriteLine("\t[3] double - 15-16 digits (64 bit)");
            WriteLine("\t[4] decimal - 28-29 digits (128 bit)");
            Write("-> ");

            var choice = int.Parse(ReadLine().Trim());

            switch (choice)
            {
            case 1: return typeof(int);
            case 2: return typeof(float);
            case 3: return typeof(double);
            case 4: return typeof(decimal);
            }

            throw new Exception("Invalid inputs.");
        }
    }
}