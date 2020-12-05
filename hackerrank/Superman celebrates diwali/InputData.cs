using System.Linq;

namespace Superman_celebrates_diwali
{
    class InputData
    {
        public int NumberOfBuildings { get; private set; }
        public int BuildingHeight { get; private set; }
        public int BuildingSwitchPenalty { get; private set; }

        public Building[] Buildings { get; private set; }

        private InputData() { }

        private void RemoveEmptyBuildings()
        {
            Buildings = Buildings.Where(building => building.TotalPeopleInside > 0).ToArray();
            NumberOfBuildings = Buildings.Length;
        }

        public static InputData Parse()
        {
            var inputData = new InputData();
            int[] numbers = ConsoleReader.ReadNumbers();
            inputData.NumberOfBuildings = numbers[0];
            inputData.BuildingHeight = numbers[1];
            inputData.BuildingSwitchPenalty = numbers[2];

            inputData.Buildings = new Building[inputData.NumberOfBuildings];
            for (int i = 0; i < inputData.NumberOfBuildings; i++)
            {
                numbers = ConsoleReader.ReadNumbers();
                var peopleInBuilding = numbers[0];
                var peoplePerFloor = new int[inputData.BuildingHeight];

                foreach (var personAtFloor in numbers.Skip(1))
                    peoplePerFloor[personAtFloor - 1]++;

                inputData.Buildings[i] = new Building(inputData.BuildingHeight, peopleInBuilding, peoplePerFloor);
            }
            inputData.RemoveEmptyBuildings();

            return inputData;
        }
    }
}
