use std::io;
use std::io::Write;

fn prompt(question: &str, mut input: &mut String) -> io::Result<usize> {
    print!("{} ", question);
    io::stdout().flush().expect("Failed to flush");
    io::stdin().read_line(&mut input)
}

pub fn exec(question: &str) {
    loop {
        let mut input = String::new();
        prompt(question, &mut input).expect("Failed to read line");
        let trimmed = input.trim();
        match trimmed {
            "" => {
                print!("Input cannot be empty. ");
                continue;
            },
            _ => {
                println!("{} has {} characters.", trimmed, trimmed.len());
                break;
            }
        }
    }
}
