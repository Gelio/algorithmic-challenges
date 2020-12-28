use criterion::{criterion_group, criterion_main, Criterion};
use leetcode_128::Solution;
use rand::{Rng, SeedableRng};
use rand_pcg::Pcg64;

fn benchmark_small(c: &mut Criterion) {
    run_benchmark(c, 13213, 100, 100);
}

fn benchmark_medium(c: &mut Criterion) {
    run_benchmark(c, 13214, 1000, 1000);
}

fn benchmark_large(c: &mut Criterion) {
    run_benchmark(c, 13215, 10000, 1000);
}

fn benchmark_large_and_close_together(c: &mut Criterion) {
    run_benchmark(c, 13216, 10000, 100);
}

fn run_benchmark(c: &mut Criterion, seed: u64, nums_size: u64, max_num: i32) {
    let mut rng = Pcg64::seed_from_u64(seed);
    let nums: Vec<i32> = (0..=nums_size).map(|_| rng.gen_range(0..max_num)).collect();

    c.bench_function("hashmap", |b| {
        b.iter(|| Solution::longest_consecutive(nums.clone()))
    });

    c.bench_function("array sort", |b| {
        b.iter(|| Solution::longest_consecutive_array_sort(nums.clone()))
    });
}

criterion_group!(
    benches,
    benchmark_small,
    benchmark_medium,
    benchmark_large,
    benchmark_large_and_close_together
);
criterion_main!(benches);
