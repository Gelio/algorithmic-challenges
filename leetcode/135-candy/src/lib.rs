use std::{cell::RefCell, collections::HashMap};

type Edges = RefCell<Vec<usize>>;
impl Solution {
    #[allow(dead_code)]
    pub fn candy(ratings: Vec<i32>) -> i32 {
        if ratings.is_empty() {
            return 0;
        }

        let mut edges: HashMap<usize, Edges> = HashMap::new();

        edges.insert(0, RefCell::new(Vec::new()));
        let mut prev_rating = ratings[0];

        ratings
            .iter()
            .cloned()
            .enumerate()
            .skip(1)
            .for_each(|(i, rating)| {
                if rating < prev_rating {
                    // Previous element points to the current
                    edges.get(&(i - 1)).unwrap().borrow_mut().push(i);
                    edges.insert(i, RefCell::new(Vec::new()));
                } else if rating > prev_rating {
                    // Current element points to the previous one
                    edges.insert(i, RefCell::new(vec![i - 1]));
                } else {
                    // No relationship with the previous edge
                    edges.insert(i, RefCell::new(Vec::new()));
                }

                prev_rating = rating;
            });

        let mut candies: Vec<i32> = vec![0; ratings.len()];
        let mut visited: Vec<bool> = vec![false; ratings.len()];

        fn assign_candies(
            i: usize,
            visited: &mut Vec<bool>,
            edges: &HashMap<usize, Edges>,
            candies: &mut Vec<i32>,
        ) {
            if visited[i] {
                return;
            }

            visited[i] = true;

            let neighbors = edges[&i].borrow();

            neighbors.iter().cloned().for_each(|neighbor| {
                assign_candies(neighbor, visited, edges, candies);
            });

            let max_neighbor_candies = neighbors.iter().map(|n| candies[*n]).max().unwrap_or(0);
            candies[i] = max_neighbor_candies + 1;
        };

        (0..ratings.len()).for_each(|i| {
            assign_candies(i, &mut visited, &edges, &mut candies);
        });

        candies.iter().sum()
    }
}

struct Solution {}

#[cfg(test)]
mod tests {
    use super::Solution;

    struct TestCase<'a> {
        name: &'a str,
        input: Vec<i32>,
        expected_output: i32,
    }

    impl TestCase<'_> {
        fn run(&self) {
            let result = Solution::candy(self.input.clone());

            assert_eq!(
                result, self.expected_output,
                "invalid output in {}",
                self.name
            );
        }
    }

    #[test]
    fn solution_tests() {
        let test_cases = vec![
            TestCase {
                name: "Example 1",
                input: vec![1, 0, 2],
                expected_output: 5,
            },
            TestCase {
                name: "Example 2",
                input: vec![1, 2, 2],
                expected_output: 4,
            },
            TestCase {
                name: "Long chain",
                input: vec![1, 2, 3, 4, 5, 6],
                expected_output: 21,
            },
            TestCase {
                name: "Long chain reversed",
                input: vec![6, 5, 4, 3, 2, 1],
                expected_output: 21,
            },
        ];

        test_cases.iter().for_each(|t| t.run());
    }
}
