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

struct SequencePtr {
    sequence: Rc<RefCell<Sequence>>,
}

impl Debug for SequencePtr {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        f.debug_struct("SequencePtr")
            .field("sequence", &self.sequence.borrow())
            .finish()
    }
}

struct Solution {}

impl Solution {
    #[allow(dead_code)]
    pub fn longest_consecutive(nums: Vec<i32>) -> i32 {
        Self::longest_consecutive_ref(&nums)
    }

    pub fn longest_consecutive_ref(nums: &Vec<i32>) -> i32 {
        // TODO: try using references and lifetime parameters
        // Holds the sequence of which the number is a part of
        let mut nums_set: HashMap<i32, Rc<RefCell<SequencePtr>>> = HashMap::new();
        let mut longest_sequence: Option<Rc<RefCell<Sequence>>> = None;

        let mut update_longest_sequence = |s: &Rc<RefCell<Sequence>>| match &longest_sequence {
            Some(longest_s) if longest_s.borrow().len >= s.borrow().len => {}
            _ => {
                longest_sequence = Some(Rc::clone(s));
            }
        };

        nums.iter().cloned().for_each(|num| {
            if nums_set.get(&num).is_some() {
                return;
            }

            let seq_ptr = match (nums_set.get(&(num - 1)), nums_set.get(&(num + 1))) {
                (Some(a), Some(b)) => {
                    // Join both sequences. Join only the sequences for a.from and b.to pointers
                    let from = a.borrow().sequence.borrow().from;
                    let to = b.borrow().sequence.borrow().to;
                    let joined_sequence = Self::join_sequences(&nums_set, from, to);

                    Rc::clone(joined_sequence)
                }
                (Some(seq_ptr), None) | (None, Some(seq_ptr)) => {
                    // Append/prepend to a single sequence
                    let borrowed_seq_ptr = seq_ptr.borrow();
                    let mut sequence = borrowed_seq_ptr.sequence.borrow_mut();
                    sequence.len += 1;
                    sequence.from = min(sequence.from, num);
                    sequence.to = max(sequence.to, num);

                    Rc::clone(seq_ptr)
                }
                (None, None) => {
                    let sequence = Rc::new(RefCell::new(Sequence {
                        len: 1,
                        from: num,
                        to: num,
                    }));

                    Rc::new(RefCell::new(SequencePtr { sequence }))
                }
            };

            update_longest_sequence(&seq_ptr.borrow().sequence);
            nums_set.insert(num, seq_ptr);
        });

        longest_sequence.unwrap_or_default().borrow().len
    }

    fn join_sequences(
        sequence_map: &HashMap<i32, Rc<RefCell<SequencePtr>>>,
        from: i32,
        to: i32,
    ) -> &Rc<RefCell<SequencePtr>> {
        let s1 = sequence_map.get(&from).unwrap();
        let s2 = sequence_map.get(&to).unwrap();

        let joined_sequence = Rc::new(RefCell::new(Sequence::new(from, to)));

        s2.borrow_mut().sequence = Rc::clone(&joined_sequence);
        s1.borrow_mut().sequence = joined_sequence;

        &s1
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
