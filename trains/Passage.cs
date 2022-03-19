using System;
namespace trains
{
    public class Passage
    {
        public int trainNum;
        public int departSt;
        public int arrivalSt;
        public double cost;
        public DateTime departTime;
        public DateTime arrivalTime;
        public Passage(int trainNum, int departSt, int arrivalSt, double cost, DateTime departTime, DateTime arrivalTime)
        {
            this.trainNum = trainNum;
            this.departSt = departSt;
            this.arrivalSt = arrivalSt;
            this.cost = cost;
            this.departTime = departTime;
            this.arrivalTime = arrivalTime;
        }
    }
}