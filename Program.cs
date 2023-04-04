namespace CarRace1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

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
            var volvoTask = Car.Race(volvo);
            var saabTask = Car.Race(saab);
            var bugattiTask = Car.Race(bugatti);
            var statusCarTask = CarStatus(new List<Car> { volvo, saab, bugatti });

            var carTask = new List<Task> { volvoTask, saabTask, bugattiTask };

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
                await finishedTask;
                carTask.Remove(finishedTask);
            }
        }

        public static async Task Tick(int tick = 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(tick));
        }

        public static async Task CarStatus(List<Car> car)
        {
            // Skriv ut status på alla ägg, dvs temperatur och hur länge de kokat
            while (true)
            {
                Console.ReadKey(true);
                await Task.Delay(TimeSpan.FromSeconds(0));
                //Console.Clear();
                car.ForEach(car =>
                {

                    Console.WriteLine($"{car.Name} has been in the race for {car.ElapsedTime} and has a velocity of {car.Velocity}");
                    Console.WriteLine($"And got {car.TimeToFinish} seconds remaining");
                });
                // När alla egg's remaining time är noll, avsluta simuleringen

                var totalRemaining = car.Select(car => car.TimeToFinish).Sum();

                //var totalRemaining = (from egg in eggs
                //                     let remaining = egg.RemainingTime()
                //                     select remaining).Sum();
                if (totalRemaining == 0)
                {
                    return;
                }

            }



        }
    }
}