using System.Collections.Generic;
using System.IO;
using System;
namespace trains
{
    public static class Parsing
    {
        public static List<Passage> ParseCsvToPassageList(string filePath)
        {
            List<Passage> result = new List<Passage>();
            StreamReader sr = new StreamReader(filePath);
            while(true)
            {
                string current = sr.ReadLine();
                if(current == null)
                {
                    break;
                }
                string[] parts = current.Split(';');
                if(parts.Length != 6)
                {
                    throw new Exception("Error: Csv file doesn`t have enough info.");
                }
                int departSt; DateTime departTime; int arrivalSt; DateTime arrivalTime; double cost; int trainNum;
                if(!int.TryParse(parts[0], out trainNum) || !int.TryParse(parts[1], out departSt) || !int.TryParse(parts[2], out arrivalSt))
                {
                    throw new Exception("Error: Csv doesn`t available with integer values.");
                }
                if(!double.TryParse(parts[3], out cost))
                {
                    throw new Exception("Error: Csv doesn`t available with double values.");
                }
                if(!DateTime.TryParse(parts[4], out departTime) || !DateTime.TryParse(parts[5], out arrivalTime))
                {
                    throw new Exception("Error: Csv doesn`t available with time values.");
                }
                Passage curEntity = new Passage(trainNum, departSt, arrivalSt, cost, departTime, arrivalTime);
                result.Add(curEntity);
            }
            sr.Close();
            return result;
        }
    }
}