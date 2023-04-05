namespace CarRace1
{
    internal class Car
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Velocity { get; set; }
        public double DistanceLeft { get; set; }
        public double ElapsedTime { get; set; }
        public double Penalty { get; set; }
        public double TimeToFinish { get; set; }

        public Car()
        {
            DistanceLeft = 10000;
            Velocity = 120;
            ElapsedTime = 0;
            Penalty = 0;
            TimeToFinish = DistanceLeft / (Velocity / 3.6) + Penalty;
        }
    }
}
