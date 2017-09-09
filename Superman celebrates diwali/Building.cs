using System;

namespace Superman_celebrates_diwali
{
    class Building
    {
        public int Height { get; private set; }
        public int[] PeopleAtFloor { get; private set; }

        public int TotalPeopleInside { get; private set; }

        public Building(int height, int totalPeopleInside, int[] peoplePerFloor)
        {
            Height = height;
            TotalPeopleInside = totalPeopleInside;

            if (peoplePerFloor.Length != height)
                throw new ArgumentException("The number of floors does not match the height");

            PeopleAtFloor = peoplePerFloor;
        }
    }
}
