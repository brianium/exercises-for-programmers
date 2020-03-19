use std::io;
use std::io::Write;
use std::env;

fn prompt(question: &str, mut input: &mut String) -> io::Result<usize> {
    print!("{} ", question);
    io::stdout().flush().expect("Failed to flush");
    io::stdin().read_line(&mut input)
}

struct Quote {
    author: String,
    text: String
}

impl Quote {
    fn new(text: &str, author: &str) -> Quote {
        Quote {text: text.to_string(), author: author.to_string()}
    }

    fn output(&self) {
        println!("{} says, \"{}\"", self.author.trim(), self.text.trim());
    }
}

fn output(quotes: Vec<Quote>) {
    for quote in quotes.iter() {
        quote.output();
    }
}

fn cli () {
    let mut text = String::new();
    let mut author = String::new();

    prompt("What is the quote?", &mut text).expect("Failed to read line");
    prompt("Who said it?", &mut author).expect("Failed to read line");

    let quotes = vec![Quote { text: text.to_string(), author: author.to_string() }];
    output(quotes);
}

fn multiple_quotes() {
    let quotes = vec![Quote::new("Dude! Where's my car?", "Ashton Kutcher"), Quote::new("These aren't the droids you're looking for.", "Obi-Wan Kenobi")];
    output(quotes);
}

fn main() {
    let args: Vec<String> = env::args().collect();
    let command = args.get(1);
    match command {
        Some(x) => if x == "cli" { cli(); } else { multiple_quotes(); },
        _ => {
            multiple_quotes();
        }
    }
}
