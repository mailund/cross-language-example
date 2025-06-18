pub fn give_greeting() -> String {
    "Hello from Rust!".to_string()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_give_greeting() {
        assert_eq!(give_greeting(), "Hello from Rust!");
    }
}
