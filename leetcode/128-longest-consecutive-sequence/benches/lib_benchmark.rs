use criterion::{criterion_group, criterion_main, Criterion};
use leetcode_128::Solution;
use rand::{Rng, SeedableRng};
use rand_pcg::Pcg64;

struct IncrementingSeed {
    seed: u64,
}

impl IncrementingSeed {
    fn get(&mut self) -> u64 {
        let seed = self.seed;
        self.seed += 1;

        seed
    }
}

fn benchmark(c: &mut Criterion) {
    let mut is = IncrementingSeed { seed: 13213 };
    run_benchmark(c, "small", is.get(), 100, 100);
    run_benchmark(c, "medium", is.get(), 1000, 1000);
    run_benchmark(c, "values as in problem", is.get(), 1e4 as _, 1e9 as _);
    run_benchmark(c, "large", is.get(), 1e5 as _, 1e9 as _);
    run_benchmark(c, "large and close together", is.get(), 1e5 as _, 100);
    run_benchmark(c, "extremely large", is.get(), 1e6 as _, 1e5 as _);
}

fn run_benchmark(c: &mut Criterion, name: &str, seed: u64, nums_size: u64, max_num: i32) {
    let mut rng = Pcg64::seed_from_u64(seed);
    let nums: Vec<i32> = (0..=nums_size)
        .map(|_| rng.gen_range(-max_num..max_num))
        .collect();

    c.bench_function(
        &format!(
            "{} hashmap (N={}, abs(nums[i]) <= {})",
            name, nums_size, max_num
        ),
        |b| b.iter(|| Solution::longest_consecutive(nums.clone())),
    );

    c.bench_function(
        &format!(
            "{} array sort (N={}, abs(nums[i]) <= {})",
            name, nums_size, max_num
        ),
        |b| b.iter(|| Solution::longest_consecutive_array_sort(nums.clone())),
    );
}

criterion_group!(benches, benchmark);
criterion_main!(benches);
