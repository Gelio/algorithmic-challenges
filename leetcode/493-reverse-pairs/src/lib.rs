impl Solution {
    #[allow(dead_code)]
    pub fn reverse_pairs(nums: Vec<i32>) -> i32 {
        let nums2 = nums.iter().cloned().map(|x| x as i64).collect::<Vec<_>>();
        let mut important_pairs = 0;

        nums2.iter().cloned().enumerate().for_each(|(j, vj)| {
            nums2[0..j].iter().cloned().for_each(|vi| {
                if vi > 2 * vj {
                    important_pairs += 1
                }
            })
        });

        important_pairs
    }
}

struct Solution {}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn solution_works() {
        assert_eq!(Solution::reverse_pairs(vec![1, 3, 2, 3, 1]), 2);
        assert_eq!(Solution::reverse_pairs(vec![2, 4, 3, 5, 1]), 3);
        assert_eq!(
            Solution::reverse_pairs(vec![
                2147483647, 2147483647, 2147483647, 2147483647, 2147483647, 2147483647
            ]),
            15
        );
    }
}
