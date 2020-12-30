mod grid;
use grid::Solver;

impl Solution {
    #[allow(dead_code)]
    pub fn unique_paths_iii(grid: Vec<Vec<i32>>) -> i32 {
        let mut solver = Solver::new(grid).expect("Error when constructing grid");

        solver.get_unique_paths_count(&solver.start.clone())
    }
}

struct Solution {}

#[cfg(test)]
mod tests {
    use super::Solution;

    struct TestCase {
        name: &'static str,
        grid: Vec<Vec<i32>>,
        paths: i32,
    }

    #[test]
    fn solution_tests() {
        let test_cases = vec![
            TestCase {
                name: "Example 1",
                grid: vec![vec![1, 0, 0, 0], vec![0, 0, 0, 0], vec![0, 0, 2, -1]],
                paths: 2,
            },
            TestCase {
                name: "Example 2",
                grid: vec![vec![1, 0, 0, 0], vec![0, 0, 0, 0], vec![0, 0, 0, 2]],
                paths: 4,
            },
            TestCase {
                name: "Example 3",
                grid: vec![vec![0, 1], vec![2, 0]],
                paths: 0,
            },
        ];

        for t in test_cases {
            assert_eq!(
                Solution::unique_paths_iii(t.grid),
                t.paths,
                "invalid result in {}",
                t.name
            );
        }
    }
}
