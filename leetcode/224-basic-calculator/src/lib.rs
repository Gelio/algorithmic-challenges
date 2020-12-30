use std::{iter::Peekable, str::Chars};

impl Solution {
    #[allow(dead_code)]
    pub fn calculate(s: String) -> i32 {
        let chars = s.chars().peekable();
        let mut tokenizer = Tokenizer { chars };

        tokenizer.expression().unwrap()
    }
}

struct Tokenizer<'a> {
    chars: Peekable<Chars<'a>>,
}

trait PeekableParser<Item> {
    fn consume_whitespace(&mut self);
    fn my_next_if<F>(&mut self, predicate: F) -> Option<Item>
    where
        F: Fn(Item) -> bool;
}

impl<I: Iterator<Item = char>> PeekableParser<I::Item> for Peekable<I> {
    fn consume_whitespace(&mut self) {
        while self.my_next_if(|c| c.is_whitespace()).is_some() {}
    }

    fn my_next_if<F>(&mut self, predicate: F) -> Option<I::Item>
    where
        F: Fn(I::Item) -> bool,
    {
        if let Some(c) = self.peek().cloned().filter(|&c| predicate(c)) {
            self.next();
            return Some(c);
        }

        None
    }
}

enum Op {
    Plus,
    Minus,
}

impl<'a> Tokenizer<'a> {
    fn number(&mut self) -> Option<i32> {
        let mut digits = vec![];
        self.chars.consume_whitespace();

        while let Some(digit) = self.chars.my_next_if(|c| c.is_digit(10)) {
            digits.push(digit);
        }

        let num: String = digits.into_iter().collect();
        num.parse().ok()
    }

    fn op(&mut self) -> Option<Op> {
        self.chars.consume_whitespace();

        self.chars
            .my_next_if(|c| c == '-' || c == '+')
            .and_then(|op| match op {
                '-' => Some(Op::Minus),
                '+' => Some(Op::Plus),
                _ => None,
            })
    }

    fn parens(&mut self) -> Option<i32> {
        self.chars
            .my_next_if(|c| c == '(')
            .and_then(|_| self.expression())
            .and_then(|res| {
                self.chars.consume_whitespace();

                self.chars.my_next_if(|c| c == ')').and(Some(res))
            })
    }

    fn number_or_parenthesised_expression(&mut self) -> Option<i32> {
        self.number().or_else(|| self.parens())
    }

    fn expression(&mut self) -> Option<i32> {
        let is_prefixed_with_minus = self.chars.my_next_if(|c| c == '-').is_some();
        let mut result = self.number_or_parenthesised_expression()?;
        if is_prefixed_with_minus {
            result *= -1;
        }

        loop {
            let op = match self.op() {
                Some(op) => op,
                None => break,
            };
            let next_val = self.number_or_parenthesised_expression()?;

            match op {
                Op::Plus => result += next_val,
                Op::Minus => result -= next_val,
            };
        }

        Some(result)
    }
}

struct Solution {}

#[cfg(test)]
mod tests {
    use super::Solution;

    struct TestCase {
        expression: &'static str,
        expected_output: i32,
    }

    impl TestCase {
        fn run(&self) {
            let output = Solution::calculate(String::from(self.expression));

            assert_eq!(
                output, self.expected_output,
                "invalid output for {}",
                self.expression
            );
        }
    }

    #[test]
    fn calculate_works() {
        let test_cases = vec![
            TestCase {
                expression: "1",
                expected_output: 1,
            },
            TestCase {
                expression: "1-1",
                expected_output: 0,
            },
            TestCase {
                expression: "1 + 1",
                expected_output: 2,
            },
            TestCase {
                expression: "2-1 + 2",
                expected_output: 3,
            },
            TestCase {
                expression: "(1+(4+5+2)-3)+(6+8)",
                expected_output: 23,
            },
            TestCase {
                expression: "( 1+ (4 + 5+2   )-3) +(6+8)",
                expected_output: 23,
            },
            TestCase {
                expression: "-2 + 1",
                expected_output: -1,
            },
            TestCase {
                expression: "- (3 + (4 + 5))",
                expected_output: -12,
            },
            TestCase {
                expression: "-(-(3))",
                expected_output: 3,
            },
        ];

        for test_case in test_cases {
            test_case.run();
        }
    }
}
