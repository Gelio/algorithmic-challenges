using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superman_celebrates_diwali
{
    class SupermanProblem
    {
        private readonly InputData _inputData;
        private int? _solution = null;

        public SupermanProblem(InputData inputData)
        {
            _inputData = inputData;
        }

        public int Solve()
        {
            if (_solution.HasValue)
                return _solution.Value;

            int[,] A = new int[_inputData.BuildingHeight + 1, _inputData.NumberOfBuildings + 1];
            int floorSolutionIndex = _inputData.NumberOfBuildings;

            for (int floor = 1; floor <= _inputData.BuildingHeight; floor++)
            {
                int bestFloorSolution = 0;

                for (int building = 0; building < _inputData.NumberOfBuildings; building++)
                {
                    int floorIfJumps = floor - _inputData.BuildingSwitchPenalty;
                    int bestSolutionSoFar = A[floor - 1, building];
                    if (floorIfJumps > 0)
                    {
                        int bestSolutionIfJumps = A[floorIfJumps, floorSolutionIndex];
                        bestSolutionSoFar = Math.Max(A[floor - 1, building], bestSolutionIfJumps);
                    }

                    A[floor, building] = bestSolutionSoFar + _inputData.Buildings[building].PeopleAtFloor[floor - 1];
                    if (A[floor, building] > bestFloorSolution)
                        bestFloorSolution = A[floor, building];
                }

                A[floor, floorSolutionIndex] = bestFloorSolution;
            }

            _solution = A[_inputData.BuildingHeight, floorSolutionIndex];
            return _solution.Value;
        }
    }
}
