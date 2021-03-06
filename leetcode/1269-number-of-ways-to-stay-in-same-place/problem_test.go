package main

import "testing"

func TestSolveShouldReturnResultsFromExamples(t *testing.T) {
	cases := []struct {
		steps, arrLen, expectedResult int
	}{
		{steps: 3, arrLen: 2, expectedResult: 4},
		{steps: 2, arrLen: 4, expectedResult: 2},
		{steps: 4, arrLen: 2, expectedResult: 8},
		{steps: 27, arrLen: 7, expectedResult: 127784505},
		{steps: 500, arrLen: 969997, expectedResult: 374847123},
	}
	for i, c := range cases {
		if result := Solve(c.steps, c.arrLen); result != c.expectedResult {
			t.Errorf("Example %d failed: (steps = %d, arrLen = %d) returned %d, expected %d", i+1, c.steps, c.arrLen, result, c.expectedResult)
		}
	}
}
