using System;
using System.Collections.Generic;
using System.Linq;
namespace trains
{
    public class ProcessTSP //travelling salesman problem
    {
        private double[,] matrix;
        private double[,] matrixCopy;
        private double constSum;
        private List<Tuple<int, int>> route;
        public ProcessTSP(double[,] toProcess)
        {
            this.matrix = toProcess;
            this.matrixCopy = toProcess;
            this.route = new List<Tuple<int, int>>();
            this.constSum = 0;
        }
        public List<Tuple<int, int>> GetNearestWay()
        {
            int count = 0;
            GetNullsInMatrix();
            while(count < matrixCopy.GetLength(0) - 3)
            {
                ProcessBranchingEdge();
                count++;
            }
            for(int i = 1; i < matrix.GetLength(0); i++)
            {
                for(int j = 1; j < matrix.GetLength(1); j++)
                {
                    if(matrix[i, j] != Double.PositiveInfinity)
                    {
                        Tuple<int, int> current1 = route.Where(t => t.Item2 == (int)matrix[i, 0]).First();
                        Tuple<int, int> current2 = route.Where(t => t.Item1 == (int)matrix[0, j]).First();
                        if((route.Where(t => t.Item2 == current1.Item1).Count() == 0
                            && route.Where(t => t.Item1 == (int)matrix[i, 0]).Count() == 0)
                            || (route.Where(t => t.Item1 == current2.Item2).Count() == 0
                            && route.Where(t => t.Item2 == (int)matrix[0, j]).Count() == 0))
                        {
                            if(route.Where(t => t.Item1 == (int)matrix[i, 0]).Count() == 0 &&
                            route.Where(t => t.Item2 == (int)matrix[0, j]).Count() == 0)
                                route.Add(new Tuple<int, int>((int)matrix[i, 0], (int)matrix[0, j]));
                        }
                    }
                }
            }
            return route;
        }
        public void GetNullsInMatrix()
        {
            double[] minInRows = FindMinInRows(true);
            for(int i = 1; i<matrix.GetLength(0); i++)
            {
                for(int j = 1; j<matrix.GetLength(1); j++)
                {
                    matrix[i, j] -= minInRows[i];
                }
            }
            double[] minInColumns = FindMinInColumns(true);
            for(int j = 1; j<matrix.GetLength(1); j++)
            {
                for(int i = 1; i<matrix.GetLength(0); i++)
                {
                    matrix[i, j] -= minInColumns[j];
                }
            }
            
            foreach(double d in minInRows)
            {
                constSum += d;
            }
            foreach (double d in minInColumns)
            {
                constSum += d;
            }
        }
        public void ProcessBranchingEdge()
        {
            double[] minInRows = FindMinInRows(false);
            double[] minInColumns = FindMinInColumns(false);
            double maxConstsSum = 0;
            int departSt = 0; int arrivSt = 0;
            for(int i = 1; i < matrix.GetLength(0); i++)
            {
                for(int j = 1; j < matrix.GetLength(1); j++)
                {
                    if(matrix[i, j] == 0 && minInRows[i] + minInColumns[j] > maxConstsSum)
                    {
                        maxConstsSum = minInRows[i] + minInColumns[j];
                        departSt = i;
                        arrivSt = j;
                    }
                }
            }
            route.Add(new Tuple<int, int>((int)matrix[departSt, 0], (int)matrix[0, arrivSt]));
            
            int arrivInRows = 0;
            int departInColumns = 0;
            for(int i = 0; i < matrix.GetLength(0); i++)
            {
                if(matrix[i, 0] == matrix[0, arrivSt])
                {
                    arrivInRows = i;
                }
                if(matrix[0, i] == matrix[departSt, 0])
                {
                    departInColumns = i;
                }
            }
          
            if(arrivInRows != 0 && departInColumns != 0)
            {
                matrix[arrivInRows, departInColumns] = Double.PositiveInfinity;
            }
            double[,] currentCopy = matrix;
            matrix = new double[currentCopy.GetLength(0) - 1, currentCopy.GetLength(1) - 1];
            for(int i = 0, k = 0; i < currentCopy.GetLength(0); i++)
            {
                if(i == departSt)
                {
                    continue;
                }
                for(int j = 0, v = 0; j < currentCopy.GetLength(1); j++)
                {
                    if(j == arrivSt)
                    {
                        continue;
                    }
                    matrix[k, v] = currentCopy[i, j];
                    v++;
                }
                k++;
            }
            
            double[] currMinRowConsts = FindMinInRows(true);
            double[] currMinColumnConsts = FindMinInColumns(true);
            double currConstsSum = 0;
            foreach (double d in currMinRowConsts)
            {
                currConstsSum += d;
            }
            foreach (double d in currMinColumnConsts)
            {
                currConstsSum += d;
            }
            
            if(currConstsSum > constSum)
            {
                matrix = matrixCopy;
                GetNullsInMatrix();
                route = new List<Tuple<int, int>>();
                GetNearestWay();
            }
        }
        private double[] FindMinInRows(bool withZero)
        {
            double[] minInRows = new double[matrix.GetLength(0)];
            for(int i = 1; i<matrix.GetLength(0); i++)
            {
                double currentMin = Double.PositiveInfinity;
                for(int j = 1; j<matrix.GetLength(1); j++)
                {
                    if(matrix[i, j] < currentMin)
                    {
                        if(matrix[i, j] != 0)
                        {
                            currentMin = matrix[i, j];
                        }
                        else if(withZero)
                        {
                            currentMin = matrix[i, j];
                        }
                    }
                }
                if(currentMin != Double.PositiveInfinity)
                {
                    minInRows[i] = currentMin;
                }
            }
            return minInRows;
        }
        private double[] FindMinInColumns(bool withZero)
        {
            double[] minInColumns = new double[matrix.GetLength(1)];
            for(int j = 1; j<matrix.GetLength(1); j++)
            {
                double currentMin = Double.PositiveInfinity;
                for(int i = 1; i<matrix.GetLength(0); i++)
                {
                    if(matrix[i, j] < currentMin)
                    {
                        if(matrix[i, j] != 0)
                        {
                            currentMin = matrix[i, j];
                        }
                        else if(withZero)
                        {
                            currentMin = matrix[i, j];
                        }
                    }
                }
                if(currentMin != Double.PositiveInfinity)
                {
                    minInColumns[j] = currentMin;
                }
            }
            return minInColumns;
        }
    }
}