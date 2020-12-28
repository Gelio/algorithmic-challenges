use std::{
    cell::RefCell,
    cmp::{max, min},
    collections::HashMap,
    fmt::Debug,
    rc::Rc,
};

#[derive(Debug)]
struct Sequence {
    len: i32,
    from: i32,
    to: i32,
}

impl Sequence {
    fn new(from: i32, to: i32) -> Self {
        Self {
            from,
            to,
            len: to - from + 1,
        }
    }
}

impl Default for Sequence {
    fn default() -> Self {
        Sequence {
            from: 0,
            to: 0,
            len: 0,
        }
    }
}

pub struct Solution {}

impl Solution {
    #[allow(dead_code)]
    pub fn longest_consecutive(nums: Vec<i32>) -> i32 {
        Self::longest_consecutive_ref(&nums)
    }

    pub fn longest_consecutive_ref(nums: &Vec<i32>) -> i32 {
        // Holds the sequence of which the number is a part of
        let mut num_sequences: HashMap<i32, Rc<RefCell<Sequence>>> = HashMap::new();
        let mut longest_sequence_len: Option<i32> = None;

        let mut update_longest_sequence = |sequence_len: i32| match &longest_sequence_len {
            Some(longest_s) if *longest_s >= sequence_len => {}
            _ => {
                longest_sequence_len = Some(sequence_len);
            }
        };

        nums.iter().cloned().for_each(|num| {
            if num_sequences.get(&num).is_some() {
                return;
            }

            let sequence = match (num_sequences.get(&(num - 1)), num_sequences.get(&(num + 1))) {
                (Some(a), Some(b)) => {
                    // Join both sequences. Join only the sequences for a.from and b.to pointers
                    let from = a.borrow().from;
                    let to = b.borrow().to;

                    Self::join_sequences(&mut num_sequences, from, to)
                }
                (Some(seq_ref), None) | (None, Some(seq_ref)) => {
                    // Append/prepend to a single sequence
                    let mut sequence = seq_ref.borrow_mut();
                    sequence.len += 1;
                    sequence.from = min(sequence.from, num);
                    sequence.to = max(sequence.to, num);

                    Rc::clone(seq_ref)
                }
                (None, None) => Rc::new(RefCell::new(Sequence::new(num, num))),
            };

            update_longest_sequence(sequence.borrow().len);
            num_sequences.insert(num, sequence);
        });

        longest_sequence_len.unwrap_or_default()
    }

    fn join_sequences(
        sequence_map: &mut HashMap<i32, Rc<RefCell<Sequence>>>,
        from: i32,
        to: i32,
    ) -> Rc<RefCell<Sequence>> {
        let joined_sequence = Rc::new(RefCell::new(Sequence::new(from, to)));

        sequence_map.insert(from, Rc::clone(&joined_sequence));
        sequence_map.insert(to, Rc::clone(&joined_sequence));

        joined_sequence
    }

    pub fn longest_consecutive_array_sort(mut nums: Vec<i32>) -> i32 {
        if nums.is_empty() {
            return 0;
        }

        nums.sort();
        let mut longest_sequence_len = 1;
        let mut sequence_len = 1;
        let mut prev = nums[0];

        nums[1..].iter().cloned().for_each(|num| {
            if num == prev || num == prev + 1 {
                sequence_len += 1;
                prev = num;
                if sequence_len > longest_sequence_len {
                    longest_sequence_len = sequence_len;
                }
            }
        });

        longest_sequence_len
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    struct TestCase {
        input: Vec<i32>,
        expected_output: i32,
    }

    impl TestCase {
        fn run(&self) {
            let output = Solution::longest_consecutive_ref(&self.input);

            assert_eq!(output, self.expected_output);
        }
    }

    #[test]
    fn examples() {
        let cases = vec![
            TestCase {
                input: vec![100, 4, 200, 1, 3, 2],
                expected_output: 4,
            },
            TestCase {
                input: vec![0, 3, 7, 2, 5, 8, 4, 6, 0, 1],
                expected_output: 9,
            },
            TestCase {
                input: vec![],
                expected_output: 0,
            },
        ];

        for c in cases {
            c.run();
        }
    }
}
