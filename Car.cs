namespace CarRace
{
    internal class Car
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Velocity { get; set; } //km/h
        public double DistanceLeft { get; set; } //meters
        public double ElapsedTime { get; set; } //Seconds
        public double Penalty { get; set; } //Seconds
        public double TimeToFinish { get; set; } //Seconds

        //Sets default values
        public Car()
        {
            DistanceLeft = 10000; //meters
            Velocity = 120; //km/h
            ElapsedTime = 0; //Seconds
            Penalty = 0; //Seconds
            TimeToFinish = DistanceLeft / (Velocity / 3.6) + Penalty; //Seconds
        }
    }
}
