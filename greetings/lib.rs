#[no_mangle]
pub extern "C" fn get_greeting() -> *const std::os::raw::c_char {
    std::ffi::CString::new("Hello from Rust!").unwrap().into_raw()
}

/// Frees a C string previously created by Rust.
///
/// # Safety
///
/// This function is unsafe because it dereferences a raw pointer.
/// The pointer must have been previously returned by a call to get_greeting().
/// After this call, the pointer is invalid and should not be used.
#[no_mangle]
pub unsafe extern "C" fn free_greeting(ptr: *mut std::os::raw::c_char) {
    if !ptr.is_null() {
        let _ = std::ffi::CString::from_raw(ptr as *mut _);
        // CString will be dropped here, freeing the memory
    }
}
