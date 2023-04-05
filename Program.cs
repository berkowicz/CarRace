namespace CarRace
{
    internal class Program
    {
        //Set tick interval in simulation (seconds)
        public static int tick = 5;
        //Set elapsed time per tick in simulation (seconds)
        public static int elapsedTimePerTick = 30;

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
                Id = 0,
                Name = "Volvo",
            };
            Car saab = new Car
            {
                Id = 1,
                Name = "Saab"
            };
            Car bugatti = new Car
            {
                Id = 2,
                Name = "Bugatti"
            };
            //Tasks
            var volvoTask = Race(volvo);
            var saabTask = Race(saab);
            var bugattiTask = Race(bugatti);
            var statusCarTask = CarStatus(new List<Car> { volvo, saab, bugatti });
            var carTask = new List<Task> { volvoTask, saabTask, bugattiTask, statusCarTask };

            bool raceHasWinner = false; //Bool that determines winner

            //When any car is still in race keep loop
            while (carTask.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(carTask); //Task over all the tasks
                if (finishedTask == volvoTask) //Volvo finishes
                {
                    if (raceHasWinner == false) //Winner
                    {
                        raceHasWinner = true;
                        Console.WriteLine($"Vinner!!!!! {volvo.Name} has finished in {volvo.ElapsedTime}.");
                    }
                    else //Not winner
                        Console.WriteLine($"{volvo.Name} has finished in {volvo.ElapsedTime}.");
                }
                else if (finishedTask == saabTask) //Saab finishes
                {
                    if (raceHasWinner == false) //Winner
                    {
                        raceHasWinner = true;
                        Console.WriteLine($"Vinner!!!!! {saab.Name} has finished in {saab.ElapsedTime}.");
                    }
                    else //Not winner
                        Console.WriteLine($"{saab.Name} has finished in {saab.ElapsedTime}.");
                }
                else if (finishedTask == bugattiTask) //Bugatti finishes
                {
                    if (raceHasWinner == false) //Winner
                    {
                        raceHasWinner = true;
                        Console.WriteLine($"Vinner!!!!! {bugatti.Name} has finished in {bugatti.ElapsedTime}.");
                    }
                    else //Not winner
                        Console.WriteLine($"{bugatti.Name} has finished in {bugatti.ElapsedTime}.");
                }
                else if (finishedTask == statusCarTask) //All Tasks are returned/completed
                {
                    Console.WriteLine($"All cars finished!");
                }
                await finishedTask; //Runs when any task in carTask is returned
                carTask.Remove(finishedTask); //Removes finished car from carTask List
            }
        }

        public static async Task Tick(int tick)
        {
            await Task.Delay(TimeSpan.FromSeconds(tick));
        }

        //Prints racestatus to console
        //Here im using Console.KeyAvailable to not halt the loop with a ReadKey.
        //The method would not end when race is with ReadKey until keypress.
        //Console.KeyAvailable solves that, it waits for keypress for 3 seconds then break loop.
        public static async Task CarStatus(List<Car> cars)
        {
            while (true)
            {
                await Task.Delay(100); //If not here the task vill run syncronously

                DateTime start = DateTime.Now;
                bool gotKey = false;
                while ((DateTime.Now - start).TotalSeconds < Program.tick / 2) //True for tick/2 seconds
                {
                    if (Console.KeyAvailable) //Loops until key is pressed
                    {
                        gotKey = true;
                        break;
                    }
                }
                if (gotKey)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Enter) //Print when press Enter
                    {
                        cars.ForEach(car => //Print each car
                        {
                            PrintCar(car);
                        });
                        gotKey = false;
                    }
                }
                var totalRemaining = cars.Select(car => car.TimeToFinish).Sum(); //Checks for combined time remaning on all cars
                if (totalRemaining == 0) //Ends loop when all cars finished
                {
                    return; //Ends task
                }
            }
        }

        //Updates car data and run event
        public static async Task<Car> Race(Car car)
        {
            while (true)
            {
                await Tick(Program.tick);
                car.TimeToFinish = (car.DistanceLeft / (car.Velocity / 3.6)) + car.Penalty; //Calculate time to finish from current tick.
                if (car.TimeToFinish <= Program.elapsedTimePerTick) //Checks if time to finish is less then 1 tick. If true make car finish.
                {
                    car.ElapsedTime += car.TimeToFinish; //Adds time to finish to elapsed time. To get total time for car in race.
                    car.DistanceLeft = 0; //Sets distance to zero
                    car.TimeToFinish = 0; //Sets time to finish to zero
                    return car;
                }
                car.DistanceLeft -= (Program.elapsedTimePerTick - car.Penalty) * (car.Velocity / 3.6); //calculate new distance left from current tick.
                car.ElapsedTime += Program.elapsedTimePerTick; //Adds 1 tick of time to elapsed time
                EventRoll(car); //Initiates event
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
            //Stops for 1 tick seconds
            car.Penalty = Program.elapsedTimePerTick;
            Console.WriteLine($"{car.Name} is out of gas, {car.Penalty} second stop");
        }

        public static void Punkture(Car car)
        {
            //Stops for 2/3 tick seconds
            car.Penalty = Program.elapsedTimePerTick * (2 / 3);
            Console.WriteLine($"{car.Name} got a punkture, {car.Penalty} second stop");
        }

        public static void BirdOnScreen(Car car)
        {
            //Stops for 1/3 tick seconds
            car.Penalty = Program.elapsedTimePerTick / 3;
            Console.WriteLine($"{car.Name} got a bird on screen, {car.Penalty} second stop");
        }

        public static void EngineFailure(Car car)
        {
            //Velocity -1 km/h
            car.Velocity -= 1;
            Console.WriteLine($"{car.Name} got an enginefailure, speed reduced by 1 km/h");
        }

        public static void PrintCar(Car car)
        {
            Console.WriteLine($"{car.Name} is going {car.Velocity} km/h and has {car.DistanceLeft} meters left. {car.Penalty} Penalty");
        }
    }
}
