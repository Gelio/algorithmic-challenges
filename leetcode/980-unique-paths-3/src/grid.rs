use std::{collections::HashMap, fmt::Display};

#[derive(PartialEq, Eq)]
pub enum Tile {
    Empty,
    Blocked,
}

#[derive(PartialEq, Debug, Clone, Eq, Hash)]
pub struct Point {
    x: usize,
    y: usize,
}

#[derive(Debug, Clone)]
pub struct SolverError(String);

impl Display for SolverError {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "SolverError: {}", self.0)
    }
}

pub struct Solver {
    grid: Vec<Vec<Tile>>,
    empty_tiles_count: usize,
    pub start: Point,
    destination: Point,
    visited: HashMap<Point, bool>,
}

impl Solver {
    pub fn new(grid: Vec<Vec<i32>>) -> Result<Self, SolverError> {
        let mut start: Option<Point> = None;
        let mut destination: Option<Point> = None;
        let parsed_grid: Vec<Vec<Tile>> = grid
            .iter()
            .enumerate()
            .map(|(y, row)| {
                row.iter()
                    .enumerate()
                    .map(|(x, cell)| match cell {
                        0 => Tile::Empty,
                        -1 => Tile::Blocked,
                        1 => {
                            start = Some(Point { x, y });
                            Tile::Empty
                        }
                        2 => {
                            destination = Some(Point { x, y });
                            Tile::Empty
                        }
                        _ => panic!("Invalid cell {} at {:?}", cell, Point { x, y }),
                    })
                    .collect()
            })
            .collect();

        let start = start.ok_or(SolverError("Start point not found".to_owned()))?;
        let destination =
            destination.ok_or(SolverError("Destination point not found".to_owned()))?;

        Ok(Self {
            start,
            destination,
            empty_tiles_count: parsed_grid
                .iter()
                .map(|row| -> usize { row.iter().filter(|x| **x == Tile::Empty).count() })
                .sum(),
            grid: parsed_grid,
            visited: HashMap::new(),
        })
    }

    pub fn get_unique_paths_count(&mut self, p: &Point) -> i32 {
        if *p == self.destination {
            if self.visited.len() == self.empty_tiles_count - 1 {
                return 1;
            } else {
                return 0;
            }
        }

        self.visited.insert(p.clone(), true);

        let count = self
            .get_point_neighbors(&p)
            .iter()
            .map(|neighbor| self.get_unique_paths_count(&neighbor))
            .sum();

        self.visited.remove(&p);

        count
    }

    fn is_blocked(&self, p: &Point) -> bool {
        self.grid[p.y][p.x] == Tile::Blocked
    }

    fn get_point_neighbors(&self, p: &Point) -> Vec<Point> {
        let mut neighbors = Vec::with_capacity(4);

        let mut add_possible_neighbor = |p: Point| {
            if self.visited.get(&p).is_none() && !self.is_blocked(&p) {
                neighbors.push(p);
            }
        };

        if p.x > 0 {
            add_possible_neighbor(Point { x: p.x - 1, y: p.y });
        }
        if p.x < self.grid[0].len() - 1 {
            add_possible_neighbor(Point { x: p.x + 1, y: p.y });
        }
        if p.y > 0 {
            add_possible_neighbor(Point { x: p.x, y: p.y - 1 });
        }
        if p.y < self.grid.len() - 1 {
            add_possible_neighbor(Point { x: p.x, y: p.y + 1 });
        }

        neighbors
    }
}
