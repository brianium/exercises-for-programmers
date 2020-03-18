use std::env;
use counter;

fn main() {
    let args: Vec<String> = env::args().collect();
    let item = args.get(1);

    match item {
        Some(ui) => if ui == "gui" { counter::gui::exec(); } else { counter::cli::exec("What is the input string?") },
        None => counter::cli::exec("What is the input string?")
    };
}
