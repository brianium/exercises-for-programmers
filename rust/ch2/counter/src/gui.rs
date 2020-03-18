use druid::widget::{Flex, Label, TextBox, Padding};
use druid::{AppLauncher, Widget, WindowDesc, LocalizedString};

fn ui_builder() -> impl Widget<String> {
    let label = Label::new(|data: &String, _env: &_| format!("Characters: {}", data.len()));
    let textbox = TextBox::new();

    Flex::column()
    .with_child(Padding::new(5.0, label), 1.0)
    .with_child(Padding::new(5.0, textbox), 1.0)
}

pub fn exec() {
    let main_window = WindowDesc::new(ui_builder)
        .title(LocalizedString::new("counter-title").with_placeholder("Counter 5000".to_string()))
        .window_size((400.0, 100.0));
    AppLauncher::with_window(main_window)
        .use_simple_logger()
        .launch("".to_string())
        .expect("Launch Failed");
}
