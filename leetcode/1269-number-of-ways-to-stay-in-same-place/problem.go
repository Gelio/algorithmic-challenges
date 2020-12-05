package main

const maxSteps int = 500
const maxReachableArrayLength = maxSteps/2 + 1
const moduloDivisor int = 1e9 + 7

// Solve returns the problem solution
func Solve(steps, arrLen int) int {
	pathsFound := make([][]int, steps+1)
	reachableArrayLen := min(arrLen, maxReachableArrayLength)

	for step := 0; step <= steps; step++ {
		pathsFound[step] = make([]int, reachableArrayLen)

		if step == 0 {
			pathsFound[step][0] = 1
			continue
		}

		previousStep := step - 1
		maxReachedIndex := min(step, reachableArrayLen-1)
		for position := 0; position <= maxReachedIndex; position++ {
			// When staying on the same position
			possibleWays := pathsFound[previousStep][position]

			if position > 0 {
				// When moving right
				possibleWays += pathsFound[previousStep][position-1]
				// possibleWays %= moduloDivisor
			}

			if position < maxReachedIndex {
				// When moving left
				possibleWays += pathsFound[previousStep][position+1]
				// possibleWays %= moduloDivisor
			}

			pathsFound[step][position] = possibleWays % moduloDivisor
		}
	}

	return pathsFound[steps][0] % moduloDivisor
}

func min(x, y int) int {
	if x < y {
		return x
	} else {
		return y
	}
}
