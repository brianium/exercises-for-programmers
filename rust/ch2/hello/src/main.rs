use std::io;
use std::io::Write;

fn prompt(question: &str, mut input: &mut String) -> io::Result<usize> {
    print!("{} ", question);
    io::stdout().flush().expect("Failed to flush");
    io::stdin().read_line(&mut input)
}

fn format_message(input: &str) -> String {
    match input {
        "Brian" => String::from("Greetings benevolent creator!"),
        "Jennie" => String::from("Hello Jennie! You are super rad!"),
        _ => format!("Hello, {}, nice to meet you!", &input)
    }
}

fn output_message(msg: String) {
    println!("{}", msg);
}

fn main() {
    let mut input = String::new();
    prompt("What is your name?", &mut input).expect("Failed to read line");

    let message = format_message(input.as_str().trim());

    output_message(message);
}
