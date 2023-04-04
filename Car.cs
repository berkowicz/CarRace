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
        }

        //Uppdaterar vid varje tick
        public static async Task<Car> Race(Car car)
        {
            while (true)
            {
                await Program.Tick();
                car.TimeToFinish = (car.DistanceLeft / (car.Velocity / 3.6)) + car.Penalty;
                if (car.TimeToFinish <= 30)
                {
                    car.ElapsedTime += car.TimeToFinish;
                    car.DistanceLeft = 0;
                    return car;
                }
                car.DistanceLeft -= (30 - car.Penalty) * (car.Velocity / 3.6);
                EventRoll(car);
                car.ElapsedTime += 30;
            }

        }

        public static void EventRoll(Car car)
        {
            Random rnd = new Random();
            int roll = rnd.Next(1, 101);
            if (roll <= 2) //2%
                OutOfGas(car);
            else if (roll > 2 && roll < 7) //4%
                Punkture(car);
            else if (roll > 6 && roll < 17) //10%
                BirdOnScreen(car);
            else if (roll > 16 && roll < 37) //20%
                EngineFailure(car);
            else
                car.Penalty = 0;
        }

        public static void PrintCar(Car car)
        {
            Console.WriteLine($"{car.Name} is going {car.Velocity} km/h and has {car.DistanceLeft} meters left. {car.Penalty} Penalty");
        }

        public static void OutOfGas(Car car)
        {
            //Stop 30 sec
            Console.WriteLine($"{car.Name} is out of gas, 30 second stop");
            car.Penalty = 30;
        }

        public static void Punkture(Car car)
        {
            //Stop 20 sec
            Console.WriteLine($"{car.Name} got a punkture, 20 second stop");
            car.Penalty = 20;
        }

        public static void BirdOnScreen(Car car)
        {
            //Stopp 10 sec
            Console.WriteLine($"{car.Name} got a bird on screen, 10 second stop");
            car.Penalty = 10;
        }

        public static async void EngineFailure(Car car)
        {
            //Velocity -1 km/h
            Console.WriteLine($"{car.Name} got an enginefailure, speed reduced by 1 km/h");
            car.Velocity -= 1;
        }
    }
}
