namespace CarRace1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press Enter to start the race...");
            Console.ReadKey();
            StartRace().GetAwaiter().GetResult();
        }


        public static async Task StartRace()
        {
            Console.Clear();
            Console.WriteLine($"Race have started");
            Car volvo = new Car
            {
                Id = 1,
                Name = "Volvo",
            };
            Car saab = new Car
            {
                Id = 2,
                Name = "Saab"
            };
            Car bugatti = new Car
            {
                Id = 3,
                Name = "Bugatti"
            };

            var volvoTask = Race(volvo);
            var saabTask = Race(saab);
            var bugattiTask = Race(bugatti);
            var statusCarTask = CarStatus(new List<Car> { volvo, saab, bugatti });
            //var finishedCarTask = CarFinished(new List<Car> { volvo, saab, bugatti }); //Test
            var carTask = new List<Task> { volvoTask, saabTask, bugattiTask, statusCarTask };

            while (carTask.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(carTask);
                if (finishedTask == volvoTask)
                {
                    Console.WriteLine($"{volvo.Name} has finished in {volvo.ElapsedTime}.");
                }
                else if (finishedTask == saabTask)
                {
                    Console.WriteLine($"{saab.Name} has finished in {saab.ElapsedTime}.");
                }
                else if (finishedTask == bugattiTask)
                {
                    Console.WriteLine($"{bugatti.Name} has finished in {bugatti.ElapsedTime}.");
                }
                else if (finishedTask == statusCarTask)
                {
                    Console.WriteLine($"All cars finished!");
                }
                await finishedTask;
                carTask.Remove(finishedTask);
            }
        }


        public static async Task Tick(int tick = 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(tick));
        }

        public static async Task CarStatus(List<Car> cars)
        {
            while (true)
            {
                //Console.Clear();                  
                await Task.Delay(100);

                DateTime start = DateTime.Now;

                bool gotKey = false;
                while ((DateTime.Now - start).TotalSeconds < 3)
                {
                    if (Console.KeyAvailable)
                    {
                        gotKey = true;
                        break;
                    }
                }

                if (gotKey)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        cars.ForEach(car =>
                        {
                            PrintCar(car);
                        });
                    }
                }
                var totalRemaining = cars.Select(car => car.TimeToFinish).Sum();

                if (totalRemaining == 0)
                {
                    return;
                }
            }
        }

        public static async Task<Car> Race(Car car)
        {
            while (true)
            {
                await Tick();
                car.TimeToFinish = (car.DistanceLeft / (car.Velocity / 3.6)) + car.Penalty;
                if (car.TimeToFinish <= 30)
                {
                    car.ElapsedTime += car.TimeToFinish;
                    car.DistanceLeft = 0;
                    car.TimeToFinish = 0;
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

        public static void EngineFailure(Car car)
        {
            //Velocity -1 km/h
            Console.WriteLine($"{car.Name} got an enginefailure, speed reduced by 1 km/h");
            car.Velocity -= 1;
        }

        public static void PrintCar(Car car)
        {
            Console.WriteLine($"{car.Name} is going {car.Velocity} km/h and has {car.DistanceLeft} meters left. {car.Penalty} Penalty");
        }
    }
}
